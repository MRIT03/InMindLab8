using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using Asp.Versioning;
using Hangfire;
using Hangfire.PostgreSql;
using InMindLab8.API;
using MediatR;
using InMindLab8.Application.Commands;
using InMindLab8.Application.Services;
using InMindLab8.Domain.Entities;
using InMindLab8.Infrastructure.BackgroundJobs;
using InMindLab5.Persistence.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using InMindLab5.Persistence.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using StackExchange.Redis;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);


// ✅ 1. Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug() // Log everything Debug and above (Debug, Info, Warning, Error)
    .WriteTo.Console()    // Logs to the console
    .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day) // Logs to a file, creates a new log file each day
    .WriteTo.Seq("http://localhost:5341") 
    .CreateLogger();

builder.Host.UseSerilog();
var publisher = PersistentRabbitMqPublisher
    .CreateAsync("localhost", "CourseExchange")
    .GetAwaiter()
    .GetResult();

builder.Services.AddSingleton<IMessagePublisher>(publisher);

// Add DbContext for PostgreSQL
builder.Services.AddDbContext<UmcContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("UniversityConnection")));


var redisConnection = builder.Configuration.GetValue<string>("Redis:ConnectionString");

builder.Services.AddMemoryCache();



builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnection;
});



builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(builder.Configuration.GetConnectionString("HangfireConnection"))
);
builder.Services.AddHangfireServer();


builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();


builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Authentication and Authorizatioin using Keycloack

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "http://localhost:8080/realms/UniversityRealm";
        options.Audience = "account";
        options.RequireHttpsMetadata = false;

        // This is the critical part:
        // Basically the way my tokens are generated are as follows 
        // {
        //  .... other fields ....
        // realm_access{
        //      roles = []
        // }
        // So in other words, dotnet cannot read the roles directly, I need to tell it how to parse the JSON document
        // After parsing the JSON document I need to register the roles of the user as "Role claims"
        // Role claims is how we do authorization
        
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                // 1) Grab the 'realm_access' claim from the JWT
                var realmAccess = context.Principal?.FindFirst("realm_access")?.Value;
                if (!string.IsNullOrEmpty(realmAccess))
                {
                    // 2) realmAccess is JSON like: {"roles":["Teacher","Student","offline_access","uma_authorization"]}
                    var doc = JsonDocument.Parse(realmAccess);

                    if (doc.RootElement.TryGetProperty("roles", out var rolesElement))
                    {
                        var appIdentity = new ClaimsIdentity();

                        foreach (var role in rolesElement.EnumerateArray())
                        {
                            var roleValue = role.GetString();
                            // 3) C# reads roles as "Role claims" so I have to register each one as a claim
                            appIdentity.AddClaim(new Claim(ClaimTypes.Role, roleValue));
                        }

                        // 4) Attach these new claims to the current principal (the current user)
                        context.Principal?.AddIdentity(appIdentity);
                    }
                }

                return Task.CompletedTask;
            }
        };
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("StudentOnly", policy => policy.RequireRole("student"));
    options.AddPolicy("TeacherOnly", policy => policy.RequireRole("teacher"));
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));

});





// Register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AdminCreateCourseCommand).Assembly));

// Register Repositories
builder.Services.AddScoped<IRepository<Course>, CourseRepository>();
builder.Services.AddScoped<IRepository<Admin>, AdminRepository>();
builder.Services.AddScoped<IRepository<TeacherCourse>, TeacherCourseRepository>();
builder.Services.AddScoped<IRepository<Course>, CourseRepository>();
builder.Services.AddScoped<IRepository<Enroll>, EnrollRepository>();
builder.Services.AddScoped<IRepository<Teacher>, TeacherRepository>();
builder.Services.AddScoped<IRepository<Student>, StudentRepository>();
builder.Services.AddScoped<IBackgroundJobService, BackgroundJobService>();

builder.Services.AddControllers()
    .AddOData(options =>
    {
        options.Select().Filter().OrderBy().Count()
            .AddRouteComponents("api/v{version:apiVersion}/odata", EdmModelBuilder.GetEdmModel());
    });


builder.Services.AddApiVersioning().AddMvc();
builder.Services.AddApiVersioning().AddOData();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>
    {
        new CultureInfo("en"),
        new CultureInfo("fr")
    };

    // Set French as the default culture
    options.DefaultRequestCulture = new RequestCulture("fr");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});



var app = builder.Build();

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHangfireDashboard("/hangfire"); 
app.UseHangfireServer();

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

var jobService = app.Services.GetRequiredService<IBackgroundJobService>();

RecurringJob.AddOrUpdate(
    "hourly-job",
    () => jobService.RunHourlyJob(),
    Cron.Hourly
);

RecurringJob.AddOrUpdate(
    "daily-email-job",
    () => jobService.SendDailyEmails(),
    Cron.Daily(8)
);

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
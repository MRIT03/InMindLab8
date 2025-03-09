using InMindLab8.Microservices.Students.Data;
using InMindLab8.Microservices.Students.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register necessary services.
builder.Services.AddDbContext<StudentDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IStudentRepository, StudentRepository>(); 
builder.Services.AddHostedService<CourseCreatedConsumerHostedService>();     

builder.Services.AddSingleton<PersistentRabbitMqConsumer>(sp =>
{
    var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
    return PersistentRabbitMqConsumer.CreateAsync("localhost", "CourseExchange", "hello", scopeFactory)
        .GetAwaiter().GetResult();
});

// If needed, register other services, controllers, etc.
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();
app.Run();
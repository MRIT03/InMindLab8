using InMindLab8.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Get connection string from configuration (appsettings.json or env variables)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add PostgreSQL DbContext
builder.Services.AddDbContext<StudentDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddHostedService<CourseCreatedConsumer>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
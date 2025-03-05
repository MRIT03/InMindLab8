using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using InMindLab8.Entities; // Import your entity namespace


public class StudentDbContext : DbContext
{
    public DbSet<Class> Classes { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    
    public StudentDbContext(DbContextOptions<StudentDbContext> options)
        : base(options)
    {
    }

    
}
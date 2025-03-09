using Microsoft.EntityFrameworkCore;
using InMindLab8.Domain.Entities;

namespace InMindLab5.Persistence.Data;

public class UmcContext : DbContext
{
    public UmcContext(DbContextOptions<UmcContext> options) : base(options)
    {
        
    }
    
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Enroll> Enrollments { get; set; }
    public DbSet<TeacherCourse> TeacherCourses { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Admin schema
        modelBuilder.Entity<Admin>()
            .ToTable("Admins", schema: "AdminSchema");
        
        modelBuilder.Entity<Course>()
            .HasOne(c => c.Admin) 
            .WithMany(a => a.Courses) 
            .HasForeignKey(c => c.AdminId) 
            .OnDelete(DeleteBehavior.Restrict); 
        

        // Teacher schema
        modelBuilder.Entity<Teacher>()
            .ToTable("Teachers", schema: "TeacherSchema");
        
        modelBuilder.Entity<TeacherCourse>()
            .ToTable("TeacherCourses", schema: "TeacherSchema");

        // Student schema
        modelBuilder.Entity<Student>()
            .ToTable("Students", schema: "StudentSchema");

        

        base.OnModelCreating(modelBuilder);
    }
}
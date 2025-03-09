using System.ComponentModel.DataAnnotations;

namespace InMindLab8.Domain.Entities;

public class Course
{
    private DateTime _enrollStart;
    private DateTime _enrollEnd;
    
    
    [Required]
    public int CourseId { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public int MaxNb { get; set; }
    [Required]
    public DateTime EnrollStart {
        get => _enrollStart;
        set => _enrollStart = DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }
    
    [Required]
    public DateTime EnrollEnd
    {
        get => _enrollEnd;
        set => _enrollEnd = DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }
    [Required]
    public int AdminId { get; set; } //this represents the foreign key
    public virtual Admin Admin { get; set; }  // Navigation Property
}
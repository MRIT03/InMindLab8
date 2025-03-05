using System.ComponentModel.DataAnnotations;


namespace InMindLab8.Entities;

public class Class
{
    [Key]
    public required int ClassId { get; set; }
    [Required]
    public required int StudentId { get; set; }
    [Required]
    public required int CourseId { get; set; }
    
    private DateTime _enrollDate;
    public required DateTime Date {
        get => _enrollDate;
        set => _enrollDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }
    
    
    [Range(0.0, 20.0)]
    public float? Grade {get;set;}
    
    public virtual Student Student { get; set; }
    public virtual Course Course { get; set; }
    
    
}
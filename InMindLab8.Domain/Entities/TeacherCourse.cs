using System.ComponentModel.DataAnnotations;

namespace InMindLab8.Domain.Entities;

public class TeacherCourse
{
    [Required]
    public int TeacherCourseId { get; set; }
    [Required]
    public int TeacherId { get; set; }
    [Required]
    public int CourseId { get; set; }
    [Required]
    public TimeOnly ClassStart { get; set; }
    [Required]
    public TimeOnly ClassEnd { get; set; }
    
    public Teacher Teacher { get; set; }
    public Course Course { get; set; }
}
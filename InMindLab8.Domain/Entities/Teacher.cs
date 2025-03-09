namespace InMindLab8.Domain.Entities;

public class Teacher
{
    public required int TeacherId { get; set; }
    public required string Name { get; set; }
    public required TimeOnly ScheduleStart { get; set; }
    public required TimeOnly ScheduleEnd { get; set; }
    
    public virtual ICollection<TeacherCourse> TeacherCourses { get; set; }
    
}
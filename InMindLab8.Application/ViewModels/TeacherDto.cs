namespace InMindLab8.Application.ViewModels;

public class TeacherDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public required TimeOnly ScheduleStart { get; set; }
    public required TimeOnly ScheduleEnd { get; set; }
}
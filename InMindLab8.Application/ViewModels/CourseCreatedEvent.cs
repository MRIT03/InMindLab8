namespace InMindLab8.Application.ViewModels;

public class CourseCreatedEvent
{
    public int CourseId { get; set; }
    public string Name { get; set; }
    public int MaxNb { get; set; }
}
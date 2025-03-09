namespace InMindLab8.Microservices.Students.Entities;

public class CourseCreatedEvent
{
    public int CourseId { get; set; }
    public string Name { get; set; }
    public int MaxNb { get; set; }
}
namespace InMindLab8.Application.ViewModels;

public class CourseDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int MaxStudents { get; set; }
    public DateTime EnrollementStart { get; set; }
    public DateTime EnrollementEnd { get; set; }
    
}
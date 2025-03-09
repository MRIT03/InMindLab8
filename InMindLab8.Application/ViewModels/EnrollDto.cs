using InMindLab8.Domain.Entities;

namespace InMindLab8.Application.ViewModels;

public class EnrollDto
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public int StudentId { get; set; }
    public DateTime EnrollementDate { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace InMindLab8.Microservices.Students.Entities;

public class Student
{
    public required int StudentId { get; set; }
    public required string Name { get; set; }
    public string? Email { get; set; }
    
    [Range(0.0, 20.0)]
    public float? GradePointAverage { get; set; }
    public string? ProfilePictureUrl { get; set; }
}
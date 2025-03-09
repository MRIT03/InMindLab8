namespace InMindLab8.Domain.Entities;

public class Admin
{
    public required int AdminId { get; set; }
    public required string Name { get; set; }
    
    public virtual ICollection<Course> Courses { get; set; }
}
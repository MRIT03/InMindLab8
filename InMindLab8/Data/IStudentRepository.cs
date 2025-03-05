using System.Collections.Generic;
using System.Threading.Tasks;
using InMindLab8.Entities;

namespace InMindLab8.Data;
public interface IStudentRepository
{
    Task AddStudentAsync(Student student);
    Task<Student> GetStudentByIdAsync(int studentId);
    Task<IEnumerable<Student>> GetAllStudentsAsync();
    Task AddCourseAsync(Course course);
    Task<bool> CourseExistsAsync(int courseId);
}
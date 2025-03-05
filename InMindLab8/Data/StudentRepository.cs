using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InMindLab8.Entities;
using Microsoft.EntityFrameworkCore;

namespace InMindLab8.Data;

public class StudentRepository : IStudentRepository
{
    private readonly StudentDbContext _context;

    public StudentRepository(StudentDbContext context)
    {
        _context = context;
    }

    public async Task AddStudentAsync(Student student)
    {
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();
    }

    public async Task<Student> GetStudentByIdAsync(int studentId)
    {
        return await _context.Students.FindAsync(studentId);
    }

    public async Task<IEnumerable<Student>> GetAllStudentsAsync()
    {
        return await _context.Students.ToListAsync();
    }

    public async Task AddCourseAsync(Course course)
    {
        if (!await CourseExistsAsync(course.CourseId))
        {
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> CourseExistsAsync(int courseId)
    {
        return await _context.Courses.AnyAsync(c => c.CourseId == courseId);
    }
}
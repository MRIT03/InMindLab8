using InMindLab8.Common;
using InMindLab8.Domain.Entities;
using InMindLab5.Persistence.Data.Repositories;
using MediatR;

namespace InMindLab8.Application.Commands;

public class UpdateGPAHandler : IRequestHandler<UpdateGPACommand, Result<String>>
{
    private readonly IRepository<Student> _StudentRepository;
    private readonly IRepository<Enroll> _courseRepository;

    public UpdateGPAHandler(IRepository<Student> studentRepository, IRepository<Enroll> courseRepository)
    {
        _StudentRepository = studentRepository;
        _courseRepository = courseRepository;
    }

    public async Task<Result<string>> Handle(UpdateGPACommand request, CancellationToken cancellationToken)
    {
        var students = await _StudentRepository.GetAllAsync();
        if (students.Count == 0)
        {
            return Result<string>.Failure("No students found");
        }
        var all_courses = await _courseRepository.GetAllAsync();
        if (all_courses.Count == 0)
        {
            return Result<string>.Failure("No Enrollments found");
        }
        foreach (var student in students)
        {
            float? gpa = 0;
            var enrolledCourses = all_courses.Where(x => x.StudentId == student.StudentId && x.Grade!= null);
            foreach (var course in enrolledCourses)
            {
                gpa += course.Grade.Value;
            }
            gpa /= enrolledCourses.Count();
            student.GradePointAverage = gpa;
            _StudentRepository.SaveAsync();
            
        }

        return Result<string>.Success("Updated all the students!");
    }
}
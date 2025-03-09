using InMindLab8.Application.Mappers;
using InMindLab8.Domain.Entities;
using InMindLab5.Persistence.Data.Repositories;
using InMindLab8.Application.ViewModels;
using MediatR;

namespace InMindLab8.Application.Commands;

public class TeacherCreateClassHandler : IRequestHandler<TeacherCreateClassCommand, TeacherCourseDto>
{
    private readonly IRepository<TeacherCourse> _TeacherCourseRepository;

    public TeacherCreateClassHandler(IRepository<TeacherCourse> teacherCourseRepository)
    {
        _TeacherCourseRepository = teacherCourseRepository;
    }
    
    public async Task<TeacherCourseDto> Handle(TeacherCreateClassCommand request, CancellationToken cancellationToken)
    {
        TeacherCourse newTeacherCourse = new TeacherCourse
        {
            TeacherCourseId = request.Id,
            TeacherId = request.TeacherId,
            CourseId = request.CourseId,
            ClassStart = request.ClassStart,
            ClassEnd = request.ClassEnd
        };
        
        await _TeacherCourseRepository.AddAsync(newTeacherCourse);

        return newTeacherCourse.ToDto();
    }
}
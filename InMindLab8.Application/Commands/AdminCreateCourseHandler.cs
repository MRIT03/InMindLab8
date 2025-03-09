using InMindLab8.Domain.Entities;
using InMindLab5.Persistence.Data.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using InMindLab8.Application.Mappers;
using InMindLab8.Application.Services;
using InMindLab8.Application.ViewModels;

namespace InMindLab8.Application.Commands;

public class AdminCreateCourseHandler : IRequestHandler<AdminCreateCourseCommand, CourseDto>
{
    private readonly IRepository<Course> _courseRepository;
    private readonly IRepository<Admin> _adminRepository; 
    private readonly IMessagePublisher _messagePublisher;


    public AdminCreateCourseHandler(IRepository<Course> courseRepository, IRepository<Admin> adminRepository, IMessagePublisher messagePublisher)
    {
        _courseRepository = courseRepository;
        _adminRepository = adminRepository;
        _messagePublisher = messagePublisher;
    }

    public async Task<CourseDto> Handle(AdminCreateCourseCommand request, CancellationToken cancellationToken)
    {

        
        Course newCourse = new Course
        {
            CourseId = request.CourseToBeCreated.Id,
            Name = request.CourseToBeCreated.Title,
            MaxNb = request.CourseToBeCreated.MaxStudents,
            AdminId = request.AdminId,
            EnrollStart = DateTime.SpecifyKind( request.CourseToBeCreated.EnrollementStart, DateTimeKind.Utc),
            EnrollEnd =  DateTime.SpecifyKind(request.CourseToBeCreated.EnrollementEnd, DateTimeKind.Utc),
        };

        await _courseRepository.AddAsync(newCourse);
        CourseCreatedEvent cce = new CourseCreatedEvent
        {
            CourseId = request.CourseToBeCreated.Id,
            Name = request.CourseToBeCreated.Title,
            MaxNb = request.CourseToBeCreated.MaxStudents,
        };
        _messagePublisher.PublishCourseCreated(cce);

        return request.CourseToBeCreated;
    }
}
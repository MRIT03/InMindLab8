using InMindLab8.Application.Mappers;
using InMindLab8.Common;
using InMindLab8.Domain.Entities;
using InMindLab5.Persistence.Data.Repositories;
using InMindLab8.Application.ViewModels;
using MediatR;

namespace InMindLab8.Application.Commands;

public class StudentEnrollClassHandler : IRequestHandler<StudentEnrollClassCommand, Result<EnrollDto>>
{
    private readonly IRepository<Enroll> _EnrollRepository;
    private readonly IRepository<Course> _CourseRepository;

    public StudentEnrollClassHandler(IRepository<Enroll> EnrollRepository, IRepository<Course> CourseRepository)
    {
        _EnrollRepository = EnrollRepository;
        _CourseRepository = CourseRepository;
    }
    
    public async Task<Result<EnrollDto>> Handle(StudentEnrollClassCommand request, CancellationToken cancellationToken)
    {
        Course course = _CourseRepository.Query.Single(x => x.CourseId == request.CourseId);
        
        if (request.EnrollDate >= course.EnrollStart &&
            request.EnrollDate <= course.EnrollEnd)
        {
            var newEnroll = new Enroll
            {
                EnrollId = request.EnrollId,
                CourseId = request.CourseId,
                StudentId = request.StudentId,
                Date = request.EnrollDate,
            };
            
            await _EnrollRepository.AddAsync(newEnroll);
            return Result<EnrollDto>.Success(newEnroll.ToDto());
        }

        
        return Result<EnrollDto>.Failure("Student is not enrolling in the allowed time");
    }
}
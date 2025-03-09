using InMindLab8.Domain.Entities;
using InMindLab5.Persistence.Data.Repositories;
using MediatR;

namespace InMindLab8.Application.Queries;

public class CreateMailingListHandler : IRequestHandler<CreateMailingListQuery, Dictionary<Student, Course>>
{
    private readonly IRepository<Student> _studentRepository;
    private readonly IRepository<Course> _courseRepository;
    private readonly IRepository<Enroll> _enrollRepository;

    public CreateMailingListHandler(IRepository<Student> studentRepository, IRepository<Course> courseRepository, IRepository<Enroll> enrollRepository)
    {
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
        _enrollRepository = enrollRepository;
    }

    public async Task<Dictionary<Student, Course>> Handle(CreateMailingListQuery request,
        CancellationToken cancellationToken)
    {
        var students = await _studentRepository.GetAllAsync();
        var courses = await _courseRepository.GetAllAsync();
        var enrolls = await _enrollRepository.GetAllAsync();
        Dictionary<Student, Course> mailingList = new Dictionary<Student, Course>();
        foreach (Student student in students)
        {
            foreach (Course course in courses)
            {
                if (!enrolls.Exists(x => x.StudentId == student.StudentId
                                        && x.CourseId == course.CourseId))
                {
                    mailingList.Add(student, course);
                }
            }
        }
        return mailingList;
    }
    
}
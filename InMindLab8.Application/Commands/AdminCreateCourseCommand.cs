using InMindLab8.Application.ViewModels;
using MediatR;

namespace InMindLab8.Application.Commands;

public class AdminCreateCourseCommand : IRequest<CourseDto>
{
    public int AdminId { get; set; }
    public CourseDto CourseToBeCreated { get; set; }

}
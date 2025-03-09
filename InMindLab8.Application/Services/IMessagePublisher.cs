using InMindLab8.Application.ViewModels;


namespace InMindLab8.Application.Services;

public interface IMessagePublisher
{
    Task PublishCourseCreated(CourseCreatedEvent courseCreatedEvent);
}
using InMindLab8.Domain.Entities;
using InMindLab8.Application.ViewModels;

namespace InMindLab8.Application.Mappers;

public static class TeacherCourseMapper
{
    public static TeacherCourseDto ToDto(this TeacherCourse teacherCourse)
    {
        return new TeacherCourseDto
        {
            Id = teacherCourse.TeacherCourseId,
            TeacherId = teacherCourse.TeacherId,
            CourseId = teacherCourse.CourseId,
            ClassStart = teacherCourse.ClassStart,
            ClassEnd = teacherCourse.ClassEnd,
        };
    }

    public static List<TeacherCourseDto> ToDtoList(this List<TeacherCourse> teacherCourses)
    {
        return teacherCourses.Select( tc => tc.ToDto()).ToList();
    }
}
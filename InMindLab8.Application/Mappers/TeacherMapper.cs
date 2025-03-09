using InMindLab8.Domain.Entities;
using InMindLab8.Application.ViewModels;

namespace InMindLab8.Application.Mappers;

public static class TeacherMapper
{
    public static TeacherDto ToDto(this Teacher teacher)
    {
        return new TeacherDto
        {
            Id = teacher.TeacherId,
            Name = teacher.Name,
            ScheduleStart = teacher.ScheduleStart,
            ScheduleEnd = teacher.ScheduleEnd,
        };
    }

    public static List<TeacherDto> ToDtoList(this List<Teacher> teachers)
    {
        return teachers.Select(t => t.ToDto()).ToList();
    }
}
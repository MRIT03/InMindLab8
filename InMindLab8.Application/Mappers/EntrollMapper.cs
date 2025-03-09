using InMindLab8.Domain.Entities;
using InMindLab8.Application.ViewModels;

namespace InMindLab8.Application.Mappers;

public static class EntrollMapper
{
    public static EnrollDto ToDto(this Enroll enroll)
    {
        return new EnrollDto
        {
            Id = enroll.EnrollId,
            CourseId = enroll.CourseId,
            StudentId = enroll.StudentId,
            EnrollementDate = enroll.Date,
        };
    }

    public static List<EnrollDto> ToDtoList(this List<Enroll> enrolls)
    {
        return enrolls.Select( e => e.ToDto()).ToList();
    }
}
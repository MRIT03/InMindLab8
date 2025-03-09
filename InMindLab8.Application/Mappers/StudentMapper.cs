using InMindLab8.Application.ViewModels;

namespace InMindLab8.Application.Mappers;
using Application.ViewModels;
using Domain.Entities;


public static class StudentMapper
{
    public static StudentDto ToDto(this Student student)
    {
        return new StudentDto
        {
            Id = student.StudentId,
            Name = student.Name,
        };

    }

    public static List<StudentDto> ToDtoList(this List<Student> students)
    {
        return students.Select( s => s.ToDto()).ToList();
    }
}
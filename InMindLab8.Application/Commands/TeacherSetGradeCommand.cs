using InMindLab8.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InMindLab8.Application.Commands;

public class TeacherSetGradeCommand : IRequest<Result<String>>
{
    public int CourseId { get; set; }
    public int TeacherId { get; set; }
    public int StudentId { get; set; }
    public float Grade { get; set; }
}
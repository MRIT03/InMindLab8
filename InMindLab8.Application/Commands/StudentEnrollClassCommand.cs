using MediatR;
using InMindLab8.Common;
using InMindLab8.Application.ViewModels;

namespace InMindLab8.Application.Commands;

public class StudentEnrollClassCommand : IRequest<Result<EnrollDto>>
{
    public int EnrollId { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrollDate { get; set; }
    
}
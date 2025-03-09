using InMindLab8.Application.ViewModels;
using MediatR;

namespace InMindLab8.Application.Queries;

public class RetrieveStudentQuery : IRequest<IEnumerable<StudentDto>>
{
    
}
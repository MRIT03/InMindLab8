using InMindLab8.Application.ViewModels;
using MediatR;

namespace InMindLab8.Application.Queries;

public class RetrieveStudentsQuery : IRequest<IQueryable<StudentDto>>
{
    
}
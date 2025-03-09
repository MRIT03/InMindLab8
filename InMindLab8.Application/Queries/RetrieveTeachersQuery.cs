using InMindLab8.Domain.Entities;
using InMindLab8.Application.ViewModels;
using MediatR;

namespace InMindLab8.Application.Queries;

public class RetrieveTeachersQuery : IRequest<IQueryable<TeacherDto>>
{
    
}
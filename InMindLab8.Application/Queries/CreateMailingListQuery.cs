using InMindLab8.Domain.Entities;
using MediatR;

namespace InMindLab8.Application.Queries;

public class CreateMailingListQuery : IRequest<Dictionary<Student, Course>>
{
    
}
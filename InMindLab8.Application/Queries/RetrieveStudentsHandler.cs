using InMindLab8.Application.Mappers;
using InMindLab8.Domain.Entities;
using InMindLab5.Persistence.Data.Repositories;
using InMindLab8.Application.ViewModels;
using MediatR;

namespace InMindLab8.Application.Queries;

public class RetrieveStudentsHandler : IRequestHandler<RetrieveStudentsQuery, IQueryable<StudentDto>>
{
    private readonly IRepository<Student> _StudentRepository;

    public RetrieveStudentsHandler(IRepository<Student> StudentRepository)
    {
        _StudentRepository = StudentRepository;
    }
    
    public async Task<IQueryable<StudentDto>> Handle(RetrieveStudentsQuery request, CancellationToken cancellationToken)
    {
        List<Student> students = await _StudentRepository.GetAllAsync();
        List<StudentDto> studentDtos = students.ToDtoList();
        return studentDtos.AsQueryable();
    }
}
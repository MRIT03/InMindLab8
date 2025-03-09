using InMindLab8.Application.Mappers;
using InMindLab8.Domain.Entities;
using InMindLab5.Persistence.Data.Repositories;
using InMindLab8.Application.ViewModels;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace InMindLab8.Application.Queries;

public class RetrieveAdminsHandler : IRequestHandler<RetrieveAdminsQuery, List<AdminDto>>
{
    private readonly IRepository<Admin> _repository;

    public RetrieveAdminsHandler(IRepository<Admin> repository)
    {
        _repository = repository;
    }

    public async Task<List<AdminDto>> Handle(RetrieveAdminsQuery request, CancellationToken cancellationToken)
    {
        var admins = await _repository.GetAllAsync();
        return admins.ToDtoList();
    }
}
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InMindLab8.Domain.Entities;
using InMindLab5.Persistence.Data;

namespace InMindLab5.Persistence.Data.Repositories;

public class EnrollRepository : IRepository<Enroll>
{
    private readonly UmcContext _dbContext;

    public EnrollRepository(UmcContext dbContext)
    {
        _dbContext = dbContext;
        Query = _dbContext.Enrollments;
    }

    public IQueryable<Enroll> Query { get; }

    public async Task<List<Enroll>> GetAllAsync()
    {
        return await _dbContext.Enrollments.ToListAsync();
    }

    public async Task AddAsync(Enroll entity)
    {
        await _dbContext.AddAsync(entity);
        await SaveAsync();
    }

    public async Task DeleteAsync(Enroll entity)
    {
        _dbContext.Remove(entity);
        await SaveAsync();
    }

    public async Task SaveAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using InMindLab8.Domain.Entities;
using InMindLab5.Persistence.Data;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace InMindLab5.Persistence.Data.Repositories;

public class StudentRepository : IRepository<Student>
{
    private readonly UmcContext _dbContext;
    private readonly IDistributedCache _cache;
    private readonly IConnectionMultiplexer _redis;

    public StudentRepository(UmcContext dbContext, IDistributedCache cache, IConnectionMultiplexer redis)
    {
        _dbContext = dbContext;
        Query = _dbContext.Students;
        _cache = cache;
        _redis = redis;
    }

    public IQueryable<Student> Query { get; }

    public async Task<List<Student>> GetAllAsync()
    {
        
        return await _dbContext.Students.ToListAsync();
        
    }

    public async Task AddAsync(Student entity)
    {
        await _dbContext.AddAsync(entity);
        await SaveAsync();
    }

    public async Task DeleteAsync(Student entity)
    {
        _dbContext.Remove(entity);
        await SaveAsync();
    }

    public async Task SaveAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
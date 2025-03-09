using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using InMindLab8.Domain.Entities;
using InMindLab5.Persistence.Data;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace InMindLab5.Persistence.Data.Repositories;

public class TeacherRepository : IRepository<Teacher>
{
    private readonly UmcContext _dbContext;
    private readonly IDistributedCache _cache;
    private readonly IConnectionMultiplexer _redis;
    public TeacherRepository(UmcContext dbContext, IDistributedCache cache, IConnectionMultiplexer redis)
    {
        _dbContext = dbContext;
        _cache = cache;
        _redis = redis;
        Query = _dbContext.Teachers;
    }

    public IQueryable<Teacher> Query { get; }

    public async Task<List<Teacher>> GetAllAsync()
    {
        string cacheKey = "teachers";
        string? cacheData = await _cache.GetStringAsync(cacheKey);
        
        if (!string.IsNullOrEmpty(cacheData))
        {
            return JsonSerializer.Deserialize<List<Teacher>>(cacheData);
        }

        var teachers = await _dbContext.Teachers.ToListAsync();
        if (teachers.Count > 0)
        {
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
            };
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(teachers), options);
        }
        
        return teachers;
    }

    public async Task AddAsync(Teacher entity)
    {
        await _dbContext.AddAsync(entity);
        await SaveAsync();
    }

    public async Task DeleteAsync(Teacher entity)
    {
        _dbContext.Remove(entity);
        await SaveAsync();
    }

    public async Task SaveAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
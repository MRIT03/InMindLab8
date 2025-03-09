using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using InMindLab8.Domain.Entities;
using InMindLab5.Persistence.Data;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;


namespace InMindLab5.Persistence.Data.Repositories;

public class CourseRepository : IRepository<Course>
{
    private readonly UmcContext _dbContext;
    private readonly IDistributedCache _cache;
    private readonly IConnectionMultiplexer _redis;

    public CourseRepository(UmcContext dbContext, IDistributedCache cache, IConnectionMultiplexer redis)
    {
        _dbContext = dbContext;
        Query = _dbContext.Courses;
        _cache = cache;
        _redis = redis;
    }
    
    public async Task<List<Course>> GetAllAsync()
    {
        string cacheKey = "courses";
        string? cacheData = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cacheData))
        {
            return JsonSerializer.Deserialize<List<Course>>(cacheData);
        }
        var courses = await _dbContext.Courses.ToListAsync();

        if (courses.Count > 0)
        {
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
            };
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(courses), options);
        }
        
        return courses;
    }

    public IQueryable<Course> Query { get; }
    public async Task AddAsync(Course entity)
    {
        await _dbContext.AddAsync(entity);
        await SaveAsync();
    }

    public async Task DeleteAsync(Course entity)
    {
        _dbContext.Remove(entity);
        await SaveAsync();
    }

    public async Task SaveAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
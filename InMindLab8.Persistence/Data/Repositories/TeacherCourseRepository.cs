using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InMindLab8.Domain.Entities;
using InMindLab5.Persistence.Data;

namespace InMindLab5.Persistence.Data.Repositories;

public class TeacherCourseRepository : IRepository<TeacherCourse>
{
    private readonly UmcContext _dbContext;

    public TeacherCourseRepository(UmcContext dbContext)
    {
        _dbContext = dbContext;
        Query = _dbContext.TeacherCourses;
    }

    public IQueryable<TeacherCourse> Query { get; }

    public async Task<List<TeacherCourse>> GetAllAsync()
    {
        return await _dbContext.TeacherCourses.ToListAsync();
    }

    public async Task AddAsync(TeacherCourse entity)
    {
        await _dbContext.AddAsync(entity);
        await SaveAsync();
    }

    public async Task DeleteAsync(TeacherCourse entity)
    {
        _dbContext.Remove(entity);
        await SaveAsync();
    }

    public async Task SaveAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
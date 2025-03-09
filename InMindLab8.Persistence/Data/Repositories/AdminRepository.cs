using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InMindLab8.Domain.Entities;
using InMindLab5.Persistence.Data;

namespace InMindLab5.Persistence.Data.Repositories;

public class AdminRepository : IRepository<Admin>
{
    private readonly UmcContext _dbContext;

    public AdminRepository(UmcContext dbContext)
    {
        _dbContext = dbContext;
        Query = _dbContext.Admins;
    }

    public IQueryable<Admin> Query { get; }

    public async Task<List<Admin>> GetAllAsync()
    {
        return await _dbContext.Admins.ToListAsync();
    }

    public async Task AddAsync(Admin entity)
    {
        await _dbContext.AddAsync(entity);
        await SaveAsync();
    }

    public async Task DeleteAsync(Admin entity)
    {
        _dbContext.Remove(entity);
        await SaveAsync();
    }

    public async Task SaveAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public Boolean Exists(Admin admin)
    {
        return  _dbContext.Admins.Contains(admin);
    }
}
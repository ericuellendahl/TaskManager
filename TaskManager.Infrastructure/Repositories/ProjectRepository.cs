using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interface;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<Project>> GetUserProjectsAsync(int userId)
    {
        return await _context.Projects
            .Include(p => p.Tasks)
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }

    public async Task<Project> GetByIdAsync(int projectId)
    {
        return await _context.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == projectId);
    }

    public async Task CreateAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int projectId)
    {
        var project = await _context.Projects.FindAsync(projectId);
        if (project != null)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
    }
}

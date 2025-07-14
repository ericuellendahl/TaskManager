using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interface;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _context;

    public TaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskUser>> GetProjectTasksAsync(int projectId)
    {
        return await _context.Tasks
            .Where(t => t.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task<TaskUser> GetByIdAsync(int taskId)
    {
        return await _context.Tasks
            .Include(t => t.Project)
            .Include(t => t.Comments)
            .Include(t => t.History)
            .FirstOrDefaultAsync(t => t.Id == taskId);
    }

    public async Task CreateAsync(TaskUser task)
    {
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TaskUser task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int taskId)
    {
        var task = await _context.Tasks.FindAsync(taskId);
        if (task != null)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddCommentAsync(TaskUserComment comment)
    {
        await _context.TaskComments.AddAsync(comment);
        await _context.SaveChangesAsync();
    }

    public async Task AddHistoryAsync(TaskUserHistory history)
    {
        await _context.TaskHistories.AddAsync(history);
        await _context.SaveChangesAsync();
    }
}

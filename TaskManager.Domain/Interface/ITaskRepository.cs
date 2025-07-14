using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interface;

public interface ITaskRepository
{
    Task<IEnumerable<TaskUser>> GetProjectTasksAsync(int projectId);
    Task<TaskUser> GetByIdAsync(int taskId);
    Task CreateAsync(TaskUser task);
    Task UpdateAsync(TaskUser task);
    Task DeleteAsync(int taskId);
    Task AddCommentAsync(TaskUserComment comment);
    Task AddHistoryAsync(TaskUserHistory history);
}

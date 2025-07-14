using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Interfaces;

public interface ITaskService
{
    Task<TaskDto> AddCommentAsync(int taskId, string content, int userId);
    Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto, int userId);
    Task<bool> DeleteTaskAsync(int taskId, int userId);
    Task<IEnumerable<TaskDto>> GetProjectTasksAsync(int projectId, int userId);
    Task<TaskDto> UpdateTaskAsync(int taskId, UpdateTaskDto updateTaskDto, int userId);
}

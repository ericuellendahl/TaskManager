using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interface;

namespace TaskManager.Application.Services;

public class TaskService(ITaskRepository taskRepository, IProjectRepository projectRepository) : ITaskService
{
    private readonly ITaskRepository _taskRepository = taskRepository;
    private readonly IProjectRepository _projectRepository = projectRepository;

    public async Task<IEnumerable<TaskDto>> GetProjectTasksAsync(int projectId, int userId)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null || project.UserId != userId)
            return Enumerable.Empty<TaskDto>();

        var tasks = await _taskRepository.GetProjectTasksAsync(projectId);
        return tasks.Select(t => new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            DueDate = t.DueDate,
            Status = t.Status,
            Priority = TaskUserPriority.High,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt,
            ProjectId = t.ProjectId
        });
    }

    public async Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto, int userId)
    {
        var project = await _projectRepository.GetByIdAsync(createTaskDto.ProjectId);

        if (project == null || project.UserId != userId)
            throw new UnauthorizedAccessException("Projeto não encontrado ou não pertence ao usuário.");

        if (project.HasReachedTaskLimit)
            throw new InvalidOperationException("Limite máximo de 20 tarefas por projeto atingido.");

        var task = new TaskUser
        {
            Title = createTaskDto.Title,
            Description = createTaskDto.Description,
            DueDate = createTaskDto.DueDate,
            Status = TaskUserStatus.Pending,
            Priority = TaskUserPriority.Low,
            ProjectId = createTaskDto.ProjectId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _taskRepository.CreateAsync(task);

        // Adicionar ao histórico
        await AddTaskHistoryAsync(task.Id, "Status", null, "Pending", userId);

        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            Status = TaskUserStatus.Pending,
            Priority = TaskUserPriority.Low,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
            ProjectId = task.ProjectId
        };
    }

    public async Task<TaskDto> UpdateTaskAsync(int taskId, UpdateTaskDto updateTaskDto, int userId)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);
        if (task == null)
            return null;

        var project = await _projectRepository.GetByIdAsync(task.ProjectId);
        if (project == null || project.UserId != userId)
            return null;

        // Registrar alterações no histórico
        if (task.Status != updateTaskDto.Status)
        {
            await AddTaskHistoryAsync(taskId, "Status", task.Status.ToString(), updateTaskDto.Status.ToString(), userId);
        }

        if (task.Title != updateTaskDto.Title)
        {
            await AddTaskHistoryAsync(taskId, "Title", task.Title, updateTaskDto.Title, userId);
        }

        if (task.Description != updateTaskDto.Description)
        {
            await AddTaskHistoryAsync(taskId, "Description", task.Description, updateTaskDto.Description, userId);
        }

        // Atualizar tarefa
        task.Title = updateTaskDto.Title;
        task.Description = updateTaskDto.Description;
        task.DueDate = updateTaskDto.DueDate;
        task.Status = updateTaskDto.Status;
        task.UpdatedAt = DateTime.UtcNow;

        await _taskRepository.UpdateAsync(task);

        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            Status = task.Status,
            Priority = task.Priority,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
            ProjectId = task.ProjectId
        };
    }

    public async Task<bool> DeleteTaskAsync(int taskId, int userId)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);
        if (task == null)
            return false;

        var project = await _projectRepository.GetByIdAsync(task.ProjectId);
        if (project == null || project.UserId != userId)
            return false;

        await _taskRepository.DeleteAsync(taskId);
        return true;
    }

    public async Task<TaskDto> AddCommentAsync(int taskId, string content, int userId)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);
        if (task == null)
            return null;

        var project = await _projectRepository.GetByIdAsync(task.ProjectId);
        if (project == null || project.UserId != userId)
            return null;

        var comment = new TaskUserComment
        {
            TaskId = taskId,
            Content = content,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _taskRepository.AddCommentAsync(comment);

        // Adicionar comentário ao histórico
        await AddTaskHistoryAsync(taskId, "Comment", null, content, userId);

        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            Status = task.Status,
            Priority = task.Priority,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
            ProjectId = task.ProjectId
        };
    }

    private async Task AddTaskHistoryAsync(int taskId, string propertyName, string oldValue, string newValue, int userId)
    {
        var history = new TaskUserHistory
        {
            TaskId = taskId,
            PropertyName = propertyName,
            OldValue = oldValue,
            NewValue = newValue,
            ModifiedBy = userId,
            ModifiedAt = DateTime.UtcNow
        };

        await _taskRepository.AddHistoryAsync(history);
    }
}
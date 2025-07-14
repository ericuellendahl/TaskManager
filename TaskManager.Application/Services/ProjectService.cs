using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interface;

namespace TaskManager.Application.Services;

public class ProjectService(IProjectRepository projectRepository) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;

    public async Task<IEnumerable<ProjectDto>> GetUserProjectsAsync(int userId)
    {
        var projects = await _projectRepository.GetUserProjectsAsync(userId);
        return projects.Select(p => new ProjectDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt,
            TaskCount = p.Tasks.Count,
            CompletedTaskCount = p.Tasks.Count(t => t.Status == TaskUserStatus.Completed)
        });
    }

    public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto, int userId)
    {
        var project = new Project
        {
            Name = createProjectDto.Name,
            Description = createProjectDto.Description,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _projectRepository.CreateAsync(project);

        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt,
            TaskCount = 0,
            CompletedTaskCount = 0
        };
    }

    public async Task<bool> DeleteProjectAsync(int projectId, int userId)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);

        if (project == null || project.UserId != userId)
            return false;

        if (!project.CanBeDeleted)
            throw new InvalidOperationException("Não é possível remover um projeto com tarefas pendentes. Conclua ou remova as tarefas primeiro.");

        await _projectRepository.DeleteAsync(projectId);
        return true;
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Interfaces;

public interface IProjectService
{
    Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto, int userId);
    Task<bool> DeleteProjectAsync(int projectId, int userId);
    Task<IEnumerable<ProjectDto>> GetUserProjectsAsync(int userId);
}

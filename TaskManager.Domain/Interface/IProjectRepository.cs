using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interface;

public interface IProjectRepository
{
    Task<IEnumerable<Project>> GetUserProjectsAsync(int userId);
    Task<Project> GetByIdAsync(int projectId);
    Task CreateAsync(Project project);
    Task DeleteAsync(int projectId);
}

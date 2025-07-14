using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;

namespace TaskManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController(IProjectService projectService, ITaskService taskService) : ControllerBase
{
    private readonly IProjectService _projectService = projectService;
    private readonly ITaskService _taskService = taskService;

    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var userId = GetUserId();
        var projects = await _projectService.GetUserProjectsAsync(userId);
        return Ok(projects);
    }

    [HttpGet("{projectId}/tasks")]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetProjectTasks(int projectId)
    {
        var userId = GetUserId();
        var tasks = await _taskService.GetProjectTasksAsync(projectId, userId);
        return Ok(tasks);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto createProjectDto)
    {
        var userId = GetUserId();
        var project = await _projectService.CreateProjectAsync(createProjectDto, userId);
        return CreatedAtAction(nameof(GetProjects), new { id = project.Id }, project);
    }

    [HttpDelete("{projectId}")]
    public async Task<IActionResult> DeleteProject(int projectId)
    {
        try
        {
            var userId = GetUserId();
            var success = await _projectService.DeleteProjectAsync(projectId, userId);

            if (!success)
                return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private int GetUserId()
    {
        // Simulação de usuário - em produção viria do token JWT
        return Request.Headers.ContainsKey("UserId") ?
            int.Parse(Request.Headers["UserId"]!) : 1;
    }
}
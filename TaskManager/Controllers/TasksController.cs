using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;

namespace TaskManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto createTaskDto)
    {
        try
        {
            var userId = GetUserId();
            var task = await _taskService.CreateTaskAsync(createTaskDto, userId);
            return Ok(task);
        }
        catch (UnauthorizedAccessException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{projetId}")]
    public async Task<IActionResult> GetTask(int projetId)
    {
        var userId = GetUserId();

        return Ok(await _taskService.GetProjectTasksAsync(projetId, userId));
    }

    [HttpPut("{taskId}")]
    public async Task<IActionResult> UpdateTask(int taskId, [FromBody] UpdateTaskDto updateTaskDto)
    {
        var userId = GetUserId();
        var task = await _taskService.UpdateTaskAsync(taskId, updateTaskDto, userId);

        if (task == null)
            return NotFound();

        return Ok(task);
    }

    [HttpDelete("{taskId}")]
    public async Task<IActionResult> DeleteTask(int taskId)
    {
        var userId = GetUserId();
        var success = await _taskService.DeleteTaskAsync(taskId, userId);

        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpPost("{taskId}/comments")]
    public async Task<IActionResult> AddComment(int taskId, [FromBody] AddCommentDto addCommentDto)
    {
        var userId = GetUserId();
        var task = await _taskService.AddCommentAsync(taskId, addCommentDto.Content, userId);

        if (task == null)
            return NotFound();

        return Ok(task);
    }

    private int GetUserId()
    {
        return Request.Headers.ContainsKey("UserId") ?
            int.Parse(Request.Headers["UserId"]!) : 1;
    }
}
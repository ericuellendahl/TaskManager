using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interface;
using Xunit;

namespace TaskManager.Application.Tests.Services;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly TaskService _taskService;

    public TaskServiceTests()
    {
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _taskService = new TaskService(_taskRepositoryMock.Object, _projectRepositoryMock.Object);
    }

    [Fact]
    public async Task GetProjectTasksAsync_ReturnsTasks_WhenProjectExistsAndBelongsToUser()
    {
        // Arrange
        var projectId = 1;
        var userId = 1;

        _projectRepositoryMock.Setup(x => x.GetByIdAsync(projectId))
            .ReturnsAsync(new Project { Id = projectId, UserId = userId });

        _taskRepositoryMock.Setup(x => x.GetProjectTasksAsync(projectId))
            .ReturnsAsync(new List<TaskUser>
            {
                new TaskUser { Id = 1, Title = "Task 1", Status = TaskUserStatus.Pending, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, ProjectId = projectId }
            });

        // Act
        var result = await _taskService.GetProjectTasksAsync(projectId, userId);

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public async Task GetProjectTasksAsync_ReturnsEmpty_WhenProjectDoesNotExist()
    {
        // Arrange
        var projectId = 1;
        var userId = 1;

        _projectRepositoryMock.Setup(x => x.GetByIdAsync(projectId))
            .ReturnsAsync((Project)null!);

        // Act
        var result = await _taskService.GetProjectTasksAsync(projectId, userId);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task CreateTaskAsync_ThrowsUnauthorized_WhenProjectNotFound()
    {
        var dto = new CreateTaskDto { ProjectId = 99, Title = "Task" };

        _projectRepositoryMock.Setup(x => x.GetByIdAsync(dto.ProjectId))
            .ReturnsAsync((Project)null!);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _taskService.CreateTaskAsync(dto, userId: 1));
    }

    [Fact]
    public async Task CreateTaskAsync_CreatesTask_WhenValid()
    {
        var dto = new CreateTaskDto
        {
            ProjectId = 1,
            Title = "Task Title",
            Description = "Desc",
            DueDate = DateTime.UtcNow.AddDays(2)
        };

        TaskUser? savedTask = null;

        _projectRepositoryMock.Setup(x => x.GetByIdAsync(dto.ProjectId))
            .ReturnsAsync(new Project { Id = 1, UserId = 1 });

        _taskRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<TaskUser>()))
            .Callback<TaskUser>(t => { t.Id = 123; savedTask = t; })
            .Returns(Task.CompletedTask);

        _taskRepositoryMock.Setup(x => x.AddHistoryAsync(It.IsAny<TaskUserHistory>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _taskService.CreateTaskAsync(dto, 1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(123, result.Id);
        Assert.Equal("Task Title", result.Title);
        Assert.NotNull(savedTask);
    }

    [Fact]
    public async Task UpdateTaskAsync_ReturnsNull_WhenTaskNotFound()
    {
        _taskRepositoryMock.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((TaskUser)null!);

        var result = await _taskService.UpdateTaskAsync(1, new UpdateTaskDto(), 1);
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateTaskAsync_UpdatesAndReturnsDto_WhenValid()
    {
        var task = new TaskUser
        {
            Id = 1,
            Title = "Old",
            Description = "Old Desc",
            Status = TaskUserStatus.Pending,
            ProjectId = 10,
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            UpdatedAt = DateTime.UtcNow.AddDays(-1)
        };

        var updateDto = new UpdateTaskDto
        {
            Title = "New",
            Description = "New Desc",
            DueDate = DateTime.UtcNow,
            Status = TaskUserStatus.Completed
        };

        _taskRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(task);
        _projectRepositoryMock.Setup(x => x.GetByIdAsync(task.ProjectId)).ReturnsAsync(new Project { Id = 10, UserId = 1 });

        _taskRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<TaskUser>())).Returns(Task.CompletedTask);
        _taskRepositoryMock.Setup(x => x.AddHistoryAsync(It.IsAny<TaskUserHistory>())).Returns(Task.CompletedTask);

        var result = await _taskService.UpdateTaskAsync(1, updateDto, 1);

        Assert.NotNull(result);
        Assert.Equal("New", result.Title);
        Assert.Equal(TaskUserStatus.Completed, result.Status);
    }

    [Fact]
    public async Task DeleteTaskAsync_ReturnsFalse_WhenTaskNotFound()
    {
        _taskRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((TaskUser)null!);
        var result = await _taskService.DeleteTaskAsync(1, 1);
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteTaskAsync_ReturnsTrue_WhenValid()
    {
        _taskRepositoryMock.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(new TaskUser { Id = 1, ProjectId = 99 });

        _projectRepositoryMock.Setup(x => x.GetByIdAsync(99))
            .ReturnsAsync(new Project { Id = 99, UserId = 1 });

        _taskRepositoryMock.Setup(x => x.DeleteAsync(1)).Returns(Task.CompletedTask);

        var result = await _taskService.DeleteTaskAsync(1, 1);
        Assert.True(result);
    }

    [Fact]
    public async Task AddCommentAsync_ReturnsNull_WhenTaskNotFound()
    {
        _taskRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((TaskUser)null!);
        var result = await _taskService.AddCommentAsync(1, "content", 1);
        Assert.Null(result);
    }

    [Fact]
    public async Task AddCommentAsync_AddsComment_WhenValid()
    {
        _taskRepositoryMock.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(new TaskUser { Id = 1, ProjectId = 10 });

        _projectRepositoryMock.Setup(x => x.GetByIdAsync(10))
            .ReturnsAsync(new Project { Id = 10, UserId = 1 });

        _taskRepositoryMock.Setup(x => x.AddCommentAsync(It.IsAny<TaskUserComment>())).Returns(Task.CompletedTask);
        _taskRepositoryMock.Setup(x => x.AddHistoryAsync(It.IsAny<TaskUserHistory>())).Returns(Task.CompletedTask);

        var result = await _taskService.AddCommentAsync(1, "Meu comentário", 1);
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }
}

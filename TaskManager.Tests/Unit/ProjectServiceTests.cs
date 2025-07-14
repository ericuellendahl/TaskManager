using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interface;
using Xunit;

namespace TaskManager.Application.Tests.Services;

public class ProjectServiceTests
{
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly ProjectService _projectService;

    public ProjectServiceTests()
    {
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _projectService = new ProjectService(_projectRepositoryMock.Object);
    }

    [Fact]
    public async Task GetUserProjectsAsync_ReturnsProjectsForUser()
    {
        // Arrange
        int userId = 1;
        var projects = new List<Project>
        {
            new Project
            {
                Id = 1,
                Name = "Project 1",
                Description = "Description 1",
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Tasks = new List<TaskUser>
                {
                    new TaskUser { Id = 1, Status = TaskUserStatus.Completed },
                    new TaskUser { Id = 2, Status = TaskUserStatus.Pending }
                }
            }
        };

        _projectRepositoryMock.Setup(repo => repo.GetUserProjectsAsync(userId))
            .ReturnsAsync(projects);

        // Act
        var result = await _projectService.GetUserProjectsAsync(userId);

        // Assert
        Assert.Single(result);
        var dto = result.First();
        Assert.Equal(2, dto.TaskCount);
        Assert.Equal(1, dto.CompletedTaskCount);
    }

    [Fact]
    public async Task CreateProjectAsync_ReturnsCreatedProjectDto()
    {
        // Arrange
        int userId = 1;
        var createDto = new CreateProjectDto
        {
            Name = "New Project",
            Description = "New Description"
        };

        Project? capturedProject = null;
        _projectRepositoryMock
            .Setup(repo => repo.CreateAsync(It.IsAny<Project>()))
            .Callback<Project>(p => { p.Id = 42; capturedProject = p; })
            .Returns(Task.CompletedTask);

        // Act
        var result = await _projectService.CreateProjectAsync(createDto, userId);

        // Assert
        Assert.Equal("New Project", result.Name);
        Assert.Equal("New Description", result.Description);
        Assert.Equal(42, result.Id);
        Assert.Equal(0, result.TaskCount);
        Assert.Equal(0, result.CompletedTaskCount);
        Assert.NotNull(capturedProject);
        Assert.Equal(userId, capturedProject!.UserId);
    }

    [Fact]
    public async Task DeleteProjectAsync_ReturnsFalse_WhenProjectNotFound()
    {
        // Arrange
        int projectId = 1, userId = 1;

        _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(projectId))
            .ReturnsAsync((Project)null!);

        // Act
        var result = await _projectService.DeleteProjectAsync(projectId, userId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteProjectAsync_ReturnsFalse_WhenProjectBelongsToAnotherUser()
    {
        // Arrange
        var project = new Project { Id = 1, UserId = 999 };

        _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(project.Id))
            .ReturnsAsync(project);

        // Act
        var result = await _projectService.DeleteProjectAsync(project.Id, userId: 1);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteProjectAsync_ReturnsTrue_WhenProjectIsDeletedSuccessfully()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            UserId = 1
        };

        _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(project.Id))
            .ReturnsAsync(project);

        _projectRepositoryMock.Setup(repo => repo.DeleteAsync(project.Id))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _projectService.DeleteProjectAsync(project.Id, project.UserId);

        // Assert
        Assert.True(result);
        _projectRepositoryMock.Verify(r => r.DeleteAsync(project.Id), Times.Once);
    }
}

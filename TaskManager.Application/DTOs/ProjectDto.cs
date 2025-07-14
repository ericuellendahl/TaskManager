using System;

namespace TaskManager.Application.DTOs;

public class ProjectDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int TaskCount { get; set; }

    public int CompletedTaskCount { get; set; }
}
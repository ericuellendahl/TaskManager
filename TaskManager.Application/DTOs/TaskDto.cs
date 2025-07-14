using System;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.DTOs;

public class TaskDto
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime? DueDate { get; set; }

    public TaskUserStatus Status { get; set; }

    public TaskUserPriority Priority { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int ProjectId { get; set; }
}
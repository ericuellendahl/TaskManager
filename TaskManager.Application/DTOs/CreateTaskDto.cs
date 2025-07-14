using System;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.DTOs;

public class CreateTaskDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? DueDate { get; set; }
    public int ProjectId { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


}

using System;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.DTOs;

public class UpdateTaskDto
{
    public TaskUserStatus Status { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? DueDate { get; set; }
}

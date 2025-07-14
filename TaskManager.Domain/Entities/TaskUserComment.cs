using System;

namespace TaskManager.Domain.Entities;

public class TaskUserComment
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }

    public virtual TaskUser Task { get; set; } = null!;
}
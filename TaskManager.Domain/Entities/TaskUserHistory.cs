using System;

namespace TaskManager.Domain.Entities;

public class TaskUserHistory
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public string PropertyName { get; set; } = string.Empty;
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public DateTime ModifiedAt { get; set; }
    public int ModifiedBy { get; set; }

    public virtual TaskUser Task { get; set; } = null!;
}

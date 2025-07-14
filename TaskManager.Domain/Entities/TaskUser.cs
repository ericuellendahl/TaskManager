using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Domain.Entities;

public class TaskUser
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    public DateTime? DueDate { get; set; }
    public TaskUserStatus Status { get; set; }
    public TaskUserPriority Priority { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    [Required]
    public int ProjectId { get; set; }
    public virtual Project Project { get; set; } = null!;

    public virtual ICollection<TaskUserHistory> History { get; set; } = new List<TaskUserHistory>();
    public virtual ICollection<TaskUserComment> Comments { get; set; } = new List<TaskUserComment>();
}
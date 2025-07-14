using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TaskManager.Domain.Entities;

public class Project
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    [Required]
    public int UserId { get; set; }

    public virtual ICollection<TaskUser> Tasks { get; set; } = new List<TaskUser>();

    public bool CanBeDeleted => !Tasks.Any(t => t.Status == TaskUserStatus.Pending);
    public bool HasReachedTaskLimit => Tasks.Count >= 20;
}
using System.ComponentModel.DataAnnotations;
using MyApi.Models;
using TaskStatus = MyApi.Models.TaskStatus;

namespace MyApi.DTOs;

public class CreateTaskRequest
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    public TaskStatus Status { get; set; } = TaskStatus.TODO;

    [Required]
    public TaskPriority Priority { get; set; } = TaskPriority.MEDIUM;

    public DateTime? DueDate { get; set; }

    public int? AssignedToId { get; set; }
}

public class UpdateTaskRequest
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    public TaskStatus Status { get; set; }

    [Required]
    public TaskPriority Priority { get; set; }

    public DateTime? DueDate { get; set; }

    public int? AssignedToId { get; set; }
}

public class UpdateTaskStatusRequest
{
    [Required]
    public TaskStatus Status { get; set; }
}

public class TaskResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int? AssignedToId { get; set; }
    public string? AssignedToName { get; set; }
}

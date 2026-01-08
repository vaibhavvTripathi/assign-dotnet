using System.ComponentModel.DataAnnotations;

namespace MyApi.Models;

public class TaskItem
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
    public string Title { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    [Required]
    public TaskStatus Status { get; set; } = TaskStatus.TODO;

    [Required]
    public TaskPriority Priority { get; set; } = TaskPriority.MEDIUM;

    public DateTime? DueDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Foreign key
    public int? AssignedToId { get; set; }

    // Navigation property
    public User? AssignedTo { get; set; }
}

public enum TaskStatus
{
    TODO,
    IN_PROGRESS,
    DONE
}

public enum TaskPriority
{
    LOW,
    MEDIUM,
    HIGH
}

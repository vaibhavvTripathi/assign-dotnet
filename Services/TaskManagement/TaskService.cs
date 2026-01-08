using Microsoft.EntityFrameworkCore;
using MyApi.Data;
using MyApi.DTOs;
using MyApi.Models;
using TaskStatus = MyApi.Models.TaskStatus;

namespace MyApi.Services.TaskManagement;

public class TaskService : ITaskService
{
    private readonly ApplicationDbContext _context;

    public TaskService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskResponse>> GetAllAsync(TaskStatus? status, TaskPriority? priority, int? assignedToId)
    {
        var query = _context.Tasks
            .Include(t => t.AssignedTo)
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);

        if (priority.HasValue)
            query = query.Where(t => t.Priority == priority.Value);

        if (assignedToId.HasValue)
            query = query.Where(t => t.AssignedToId == assignedToId.Value);

        var tasks = await query
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TaskResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                Priority = t.Priority,
                DueDate = t.DueDate,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
                AssignedToId = t.AssignedToId,
                AssignedToName = t.AssignedTo != null ? t.AssignedTo.Name : null
            })
            .ToListAsync();

        return tasks;
    }

    public async Task<(bool Success, TaskResponse? Task, string? Error)> GetByIdAsync(int id)
    {
        var task = await _context.Tasks
            .Include(t => t.AssignedTo)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (task == null)
        {
            return (false, null, $"Task with ID {id} not found");
        }

        var response = new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
            AssignedToId = task.AssignedToId,
            AssignedToName = task.AssignedTo?.Name
        };

        return (true, response, null);
    }

    public async Task<(bool Success, TaskResponse? Task, string? Error)> CreateAsync(CreateTaskRequest request)
    {
        // Validate AssignedToId if provided
        if (request.AssignedToId.HasValue)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == request.AssignedToId.Value);
            if (!userExists)
            {
                return (false, null, $"User with ID {request.AssignedToId.Value} not found");
            }
        }

        var task = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            Status = request.Status,
            Priority = request.Priority,
            DueDate = request.DueDate,
            AssignedToId = request.AssignedToId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        // Load the assigned user if exists
        await _context.Entry(task)
            .Reference(t => t.AssignedTo)
            .LoadAsync();

        var response = new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
            AssignedToId = task.AssignedToId,
            AssignedToName = task.AssignedTo?.Name
        };

        return (true, response, null);
    }

    public async Task<(bool Success, TaskResponse? Task, string? Error)> UpdateAsync(int id, UpdateTaskRequest request)
    {
        var task = await _context.Tasks
            .Include(t => t.AssignedTo)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (task == null)
        {
            return (false, null, $"Task with ID {id} not found");
        }

        // Validate AssignedToId if provided
        if (request.AssignedToId.HasValue)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == request.AssignedToId.Value);
            if (!userExists)
            {
                return (false, null, $"User with ID {request.AssignedToId.Value} not found");
            }
        }

        task.Title = request.Title;
        task.Description = request.Description;
        task.Status = request.Status;
        task.Priority = request.Priority;
        task.DueDate = request.DueDate;
        task.AssignedToId = request.AssignedToId;
        task.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        // Reload the assigned user
        await _context.Entry(task)
            .Reference(t => t.AssignedTo)
            .LoadAsync();

        var response = new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
            AssignedToId = task.AssignedToId,
            AssignedToName = task.AssignedTo?.Name
        };

        return (true, response, null);
    }

    public async Task<(bool Success, TaskResponse? Task, string? Error)> UpdateStatusAsync(int id, UpdateTaskStatusRequest request)
    {
        var task = await _context.Tasks
            .Include(t => t.AssignedTo)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (task == null)
        {
            return (false, null, $"Task with ID {id} not found");
        }

        task.Status = request.Status;
        task.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var response = new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
            AssignedToId = task.AssignedToId,
            AssignedToName = task.AssignedTo?.Name
        };

        return (true, response, null);
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);

        if (task == null)
        {
            return (false, $"Task with ID {id} not found");
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return (true, null);
    }
}

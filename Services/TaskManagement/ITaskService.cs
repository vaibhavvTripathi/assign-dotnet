using MyApi.DTOs;
using MyApi.Models;
using TaskStatus = MyApi.Models.TaskStatus;

namespace MyApi.Services.TaskManagement;

public interface ITaskService
{
    Task<List<TaskResponse>> GetAllAsync(TaskStatus? status, TaskPriority? priority, int? assignedToId);
    Task<(bool Success, TaskResponse? Task, string? Error)> GetByIdAsync(int id);
    Task<(bool Success, TaskResponse? Task, string? Error)> CreateAsync(CreateTaskRequest request);
    Task<(bool Success, TaskResponse? Task, string? Error)> UpdateAsync(int id, UpdateTaskRequest request);
    Task<(bool Success, TaskResponse? Task, string? Error)> UpdateStatusAsync(int id, UpdateTaskStatusRequest request);
    Task<(bool Success, string? Error)> DeleteAsync(int id);
}

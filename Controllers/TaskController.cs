using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApi.DTOs;
using MyApi.Models;
using MyApi.Services.TaskManagement;
using TaskStatus = MyApi.Models.TaskStatus;

namespace MyApi.Controllers;

[ApiController]
[Route("api/tasks")]
[Authorize]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TaskResponse>>> GetAll(
        [FromQuery] TaskStatus? status = null,
        [FromQuery] TaskPriority? priority = null,
        [FromQuery] int? assignedToId = null)
    {
        var tasks = await _taskService.GetAllAsync(status, priority, assignedToId);
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskResponse>> GetById(int id)
    {
        var (success, task, error) = await _taskService.GetByIdAsync(id);

        if (!success)
        {
            return NotFound(new ErrorResponse
            {
                Message = error!
            });
        }

        return Ok(task);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TaskResponse>> Create(CreateTaskRequest request)
    {
        var (success, task, error) = await _taskService.CreateAsync(request);

        if (!success)
        {
            return BadRequest(new ErrorResponse
            {
                Message = error!
            });
        }

        return CreatedAtAction(nameof(GetById), new { id = task!.Id }, task);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskResponse>> Update(int id, UpdateTaskRequest request)
    {
        var (success, task, error) = await _taskService.UpdateAsync(id, request);

        if (!success)
        {
            if (error!.Contains("User with ID"))
            {
                return BadRequest(new ErrorResponse
                {
                    Message = error
                });
            }

            return NotFound(new ErrorResponse
            {
                Message = error
            });
        }

        return Ok(task);
    }

    [HttpPatch("{id}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskResponse>> UpdateStatus(int id, UpdateTaskStatusRequest request)
    {
        var (success, task, error) = await _taskService.UpdateStatusAsync(id, request);

        if (!success)
        {
            return NotFound(new ErrorResponse
            {
                Message = error!
            });
        }

        return Ok(task);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, error) = await _taskService.DeleteAsync(id);

        if (!success)
        {
            return NotFound(new ErrorResponse
            {
                Message = error!
            });
        }

        return NoContent();
    }
}

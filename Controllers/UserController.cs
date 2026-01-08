using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApi.DTOs;
using MyApi.Services.UserManagement;

namespace MyApi.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResponse<UserResponse>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var response = await _userService.GetAllAsync(page, pageSize);
        return Ok(response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponse>> GetById(int id)
    {
        var (success, user, error) = await _userService.GetByIdAsync(id);

        if (!success)
        {
            return NotFound(new ErrorResponse
            {
                Message = error!
            });
        }

        return Ok(user);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResponse>> Create(CreateUserRequest request)
    {
        var (success, user, error) = await _userService.CreateAsync(request);

        if (!success)
        {
            return BadRequest(new ErrorResponse
            {
                Message = error!
            });
        }

        return CreatedAtAction(nameof(GetById), new { id = user!.Id }, user);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, error) = await _userService.DeleteAsync(id);

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

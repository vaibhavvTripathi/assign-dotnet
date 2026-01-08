using Microsoft.AspNetCore.Mvc;
using MyApi.DTOs;
using MyApi.Services.AuthManagement;

namespace MyApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        var (success, response, error) = await _authService.RegisterAsync(request);

        if (!success)
        {
            return BadRequest(new ErrorResponse
            {
                Message = error!
            });
        }

        return CreatedAtAction(nameof(Register), response);
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var (success, response, error) = await _authService.LoginAsync(request);

        if (!success)
        {
            return Unauthorized(new ErrorResponse
            {
                Message = error!
            });
        }

        return Ok(response);
    }
}

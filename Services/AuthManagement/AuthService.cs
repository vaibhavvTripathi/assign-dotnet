using Microsoft.EntityFrameworkCore;
using MyApi.Data;
using MyApi.DTOs;
using MyApi.Models;

namespace MyApi.Services.AuthManagement;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly PasswordService _passwordService;
    private readonly JwtService _jwtService;

    public AuthService(
        ApplicationDbContext context,
        PasswordService passwordService,
        JwtService jwtService)
    {
        _context = context;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    public async Task<(bool Success, AuthResponse? Response, string? Error)> RegisterAsync(RegisterRequest request)
    {
        // Check if email already exists
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
        {
            return (false, null, "Email already exists");
        }

        var user = new MyApi.Models.User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = _passwordService.HashPassword(request.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = _jwtService.GenerateToken(user);
        var expiresAt = DateTime.UtcNow.AddDays(7);

        var response = new AuthResponse
        {
            Token = token,
            ExpiresAt = expiresAt,
            User = new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            }
        };

        return (true, response, null);
    }

    public async Task<(bool Success, AuthResponse? Response, string? Error)> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !_passwordService.VerifyPassword(request.Password, user.PasswordHash))
        {
            return (false, null, "Invalid email or password");
        }

        var token = _jwtService.GenerateToken(user);
        var expiresAt = DateTime.UtcNow.AddDays(7);

        var response = new AuthResponse
        {
            Token = token,
            ExpiresAt = expiresAt,
            User = new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            }
        };

        return (true, response, null);
    }
}

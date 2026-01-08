using Microsoft.EntityFrameworkCore;
using MyApi.Data;
using MyApi.DTOs;
using MyApi.Models;

namespace MyApi.Services.UserManagement;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResponse<UserResponse>> GetAllAsync(int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 10;

        var totalCount = await _context.Users.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var users = await _context.Users
            .OrderBy(u => u.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserResponse
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email
            })
            .ToListAsync();

        return new PaginatedResponse<UserResponse>
        {
            Data = users,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }

    public async Task<(bool Success, UserResponse? User, string? Error)> GetByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return (false, null, $"User with ID {id} not found");
        }

        var response = new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };

        return (true, response, null);
    }

    public async Task<(bool Success, UserResponse? User, string? Error)> CreateAsync(CreateUserRequest request)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
        {
            return (false, null, "Email already exists");
        }

        var user = new MyApi.Models.User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = "default_hash" // Note: In real scenario, require password
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var response = new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };

        return (true, response, null);
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return (false, $"User with ID {id} not found");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return (true, null);
    }
}

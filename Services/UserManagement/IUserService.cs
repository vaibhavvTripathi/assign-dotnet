using MyApi.DTOs;

namespace MyApi.Services.UserManagement;

public interface IUserService
{
    Task<PaginatedResponse<UserResponse>> GetAllAsync(int page, int pageSize);
    Task<(bool Success, UserResponse? User, string? Error)> GetByIdAsync(int id);
    Task<(bool Success, UserResponse? User, string? Error)> CreateAsync(CreateUserRequest request);
    Task<(bool Success, string? Error)> DeleteAsync(int id);
}

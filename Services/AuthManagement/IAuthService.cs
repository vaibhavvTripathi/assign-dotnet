using MyApi.DTOs;

namespace MyApi.Services.AuthManagement;

public interface IAuthService
{
    Task<(bool Success, AuthResponse? Response, string? Error)> RegisterAsync(RegisterRequest request);
    Task<(bool Success, AuthResponse? Response, string? Error)> LoginAsync(LoginRequest request);
}

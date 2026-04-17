using AmOzon.Contracts.Requests;
using AmOzon.Contracts.Responses;

namespace AmOzon.Application.Abstractions;

public interface IAuthService
{
    Task<AuthResponse?> LoginAsync(LoginRequest request);
    Task<AuthResponse?> RegisterAsync(RegisterRequest request);
    Task<string> RefreshRoleTokenAsync(Guid userId);
}
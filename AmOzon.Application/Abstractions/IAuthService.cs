using AmOzon.Contracts.Requests;
using AmOzon.Contracts.Responses;

namespace AmOzon.Application.Abstractions;

public interface IAuthService
{
    Task<AuthResponse?> LoginAsync(LoginRequest request);
    Task<string> RefreshRoleTokenAsync(Guid userId);
}
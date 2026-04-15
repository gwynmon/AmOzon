using AmOzon.Contracts.Responses;

namespace AmOzon.Application.Abstractions;

public interface IProfileService
{
    Task<UserResponse?> GetUserProfile(Guid userId);
}
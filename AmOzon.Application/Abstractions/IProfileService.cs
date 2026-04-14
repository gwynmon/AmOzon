using AmOzon.Contracts.Responses;

namespace AmOzon.Application.Abstractions;

public interface IProfileService
{
    Task<UserProfileResponse?> GetUserProfile(Guid userId);
}
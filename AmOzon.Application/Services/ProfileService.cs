using AmOzon.Application.Abstractions;
using AmOzon.Contracts.Responses;

namespace AmOzon.Application.Services;

public class ProfileService : IProfileService
{
    private readonly IUserService _userService;
    private readonly ISellerService _sellerService;

    public ProfileService(IUserService userService, ISellerService sellerService)
    {
        _userService = userService;
        _sellerService = sellerService;
    }

    public async Task<UserProfileResponse?> GetUserProfile(Guid userId)
    {
        var user = await _userService.GetUser(userId);
        if (user == null) return null;

        // 2. Проверяем наличие продавца (1 запрос)
        var seller = await _sellerService.GetByUserId(userId);
        
        return new UserProfileResponse
        (
            user.Id,
            user.Name,
            user.Email,
            user.Age,
            seller != null
        );
    }
}
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

    public async Task<UserResponse?> GetUserProfile(Guid userId)
    {
        var user = await _userService.GetUser(userId);
        if (user == null) return null;

        var seller = await _sellerService.GetByUserId(userId);

        Guid? sellerId = null;

        if (seller != null)
        {
            sellerId = seller.Id;
        }
        
        return new UserResponse
        (
            user.Id,
            user.Name,
            user.Age,
            user.Email,
            sellerId
        );
    }
}
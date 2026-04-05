using AmOzon.Domain.Models;

namespace AmOzon.Application.Abstractions;

public interface ICartService
{
    Task<Guid> AddProductToUsersCart(Guid userId, Guid productId, int cartQuantity);
    Task<List<CartItem>> GetUsersCartItems(Guid userId);
    Task<Guid> UpdateQuantity(Guid userId, Guid itemId, int quantity);
    Task<Guid> DeleteItem(Guid userId, Guid itemId);
    Task<Guid> ClearUsersCart(Guid userId);
}
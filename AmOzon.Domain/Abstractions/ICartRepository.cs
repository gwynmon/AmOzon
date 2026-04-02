using AmOzon.Domain.Models;

namespace AmOzon.Domain.Abstractions;

public interface ICartRepository
{
    Task<Guid> AddProductToUsersCart(CartItem cartItem);
    Task<List<CartItem>> GetUsersCartItems(Guid userId);
    Task<Guid> GetById(Guid itemId);
    Task<Guid> UpdateQuantity(Guid itemId, int quantity);
    Task<Guid> DeleteItem(Guid itemId);
    Task<Guid> ClearUsersCart(Guid userId);
}
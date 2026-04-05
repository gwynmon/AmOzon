using AmOzon.Domain.Models;

namespace AmOzon.Domain.Abstractions;

public interface ICartRepository
{
    Task<Guid> AddProductToUsersCart(Guid userId, CartItem cartItem);
    Task<List<CartItem>> GetUsersCartItems(Guid userId);
    Task<CartItem?> GetById(Guid userId, Guid itemId);
    Task<Guid?> UpdateQuantity(Guid userId, Guid itemId, int quantity);
    Task<Guid?> DeleteItem(Guid userId, Guid itemId);
    Task<Guid> ClearUsersCart(Guid userId);
}
using AmOzon.Application.Abstractions;
using AmOzon.Domain.Abstractions;
using AmOzon.Domain.Models;

namespace AmOzon.Application.Services;

public class CartService(
    ICartRepository cartRepository,
    IProductRepository productRepository)
    : ICartService
{

    public async Task<Guid> AddProductToUsersCart(Guid userId, Guid productId, int cartQuantity)
    {
        var product = await productRepository.GetById(productId);

        if (product == null)
        {
            throw new ApplicationException($"Product with id {productId} not found");
        }
        
        var cartItem = CartItem.Create(
            Guid.NewGuid(), 
            cartQuantity, 
            productId, 
            userId
        );
        
        return await cartRepository.AddProductToUsersCart(userId, cartItem);
    }

    public async Task<List<CartItem>> GetUsersCartItems(Guid userId)
    {
        return await cartRepository.GetUsersCartItems(userId);
    }

    public async Task<Guid> UpdateQuantity(Guid userId, Guid itemId, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ApplicationException("Quantity cannot be zero or negative");
        }
        
        var id = await cartRepository.UpdateQuantity(userId, itemId, quantity);

        if (id == null)
        {
            throw new ApplicationException($"Product with id {itemId} not found");
        }

        return id.Value;
    }

    public async Task<Guid> DeleteItem(Guid userId, Guid itemId)
    {
        var id = await cartRepository.DeleteItem(userId, itemId);

        if (id == null)
        {
            throw new ApplicationException($"Product with id {itemId} not found");
        }

        return id.Value;
    }

    public async Task<Guid> ClearUsersCart(Guid userId)
    {
        return await cartRepository.ClearUsersCart(userId);
    }
}
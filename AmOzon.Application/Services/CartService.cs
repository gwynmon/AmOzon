using AmOzon.Application.Abstractions;
using AmOzon.Contracts;
using AmOzon.Domain.Abstractions;
using AmOzon.Domain.Models;
using AmOzon.Persistence.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AmOzon.Application.Services;

public class CartService(
    ICartRepository cartRepository,
    IProductRepository productRepository,
    IUserRepository userRepository)
    : ICartService
{

    public async Task<Guid> AddProductToUsersCart(AddProductToUsersCartRequest request)
    {
        var product = await productRepository.GetById(request.ProductId);

        if (product == null)
        {
            throw new ApplicationException($"Product with id {request.ProductId} not found");
        }
        
        var cartItem = CartItem.Create(
            Guid.NewGuid(), 
            request.CartQuantity, 
            request.ProductId, 
            request.UserId
        );
        
        return await cartRepository.AddProductToUsersCart(cartItem);
    }

    public async Task<ActionResult<List<CartItem>>> GetUsersCartItems(Guid userId)
    {
        var user = await userRepository.GetById(userId);
        
        if (user == null)
        {
            throw new ApplicationException($"User with id {userId} not found");
        }
        
        return await cartRepository.GetUsersCartItems(userId);
    }

    public async Task<Guid> UpdateQuantity(Guid itemId, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ApplicationException("Quantity cannot be zero or negative");
        }
        
        return await cartRepository.UpdateQuantity(itemId, quantity);
    }

    public async Task<Guid> DeleteItem(Guid itemId)
    {
        var item = cartRepository.GetById(itemId);

        if (item == null)
        {
            throw new ApplicationException($"Item with id {itemId} not found");
        }
        
        return await cartRepository.DeleteItem(itemId);
    }

    public async Task<Guid> ClearUsersCart(Guid userId)
    {
        var item = cartRepository.GetUsersCartItems(userId);

        if (item == null)
        {
            throw new ApplicationException($"Items for user with id {userId} not found");
        }
        
        return await cartRepository.ClearUsersCart(userId);
    }
}
using AmOzon.Domain.Abstractions;
using AmOzon.Domain.Models;
using AmOzon.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AmOzon.Persistence.Repository;

public class CartRepository(AmOzonDbContext dbContext) : ICartRepository
{
    public async Task<List<CartItem>> GetUsersCartItems(Guid userId)
    {
        var cartItemEntities = await dbContext.CartItems
            .AsNoTracking()
            .Where(ci => ci.UserId == userId)
            .ToListAsync();

        var cartItems = cartItemEntities
            .Select(ci => CartItem.Create(
                ci.Id,
                ci.CartQuantity,
                ci.ProductId,
                ci.UserId))
            .ToList();
        
        return cartItems;
    }

    public Task<Guid> GetById(Guid itemId)
    {
        var cartItemEntity = dbContext.CartItems
            .FirstOrDefault(ci => ci.Id == itemId);
        
        return Task.FromResult(cartItemEntity.Id);
    }

    public async Task<Guid> AddProductToUsersCart(CartItem cartItem)
    {
        var cartItemEntity = new CartItemEntity
        {
            Id = cartItem.Id,
            CartQuantity = cartItem.CartQuantity,
            ProductId = cartItem.ProductId,
            UserId = cartItem.UserId
        };
        
        await dbContext.CartItems.AddAsync(cartItemEntity);
        await dbContext.SaveChangesAsync();
        return cartItemEntity.Id;
    }

    public async Task<Guid> UpdateQuantity(Guid itemId, int quantity)
    {
        var cartItemEntity = await dbContext.CartItems
            .FirstOrDefaultAsync(ci => ci.Id == itemId);

        cartItemEntity.CartQuantity = quantity;
        await dbContext.SaveChangesAsync();
        return cartItemEntity.Id;
    }

    public async Task<Guid> DeleteItem(Guid itemId)
    {
        await dbContext.CartItems
            .Where(ci => ci.Id == itemId)
            .ExecuteDeleteAsync();
        
        await dbContext.SaveChangesAsync();
        return itemId;
    }

    public async Task<Guid> ClearUsersCart(Guid userId)
    {
        await dbContext.CartItems
            .Where(ci => ci.UserId == userId)
            .ExecuteDeleteAsync();
        
        await dbContext.SaveChangesAsync();
        return userId;
    }
}
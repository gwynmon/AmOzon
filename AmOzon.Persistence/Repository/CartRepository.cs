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

    public async Task<CartItem?> GetById(Guid userId, Guid itemId)
    {
        var cartItemEntity = await dbContext.CartItems
            .FirstOrDefaultAsync(ci => ci.Id == itemId && ci.UserId == userId);

        if (cartItemEntity == null)
        {
            return null;
        }

        var cartItem = CartItem.Create(
            cartItemEntity.Id,
            cartItemEntity.CartQuantity,
            cartItemEntity.ProductId,
            cartItemEntity.UserId
        );
        
        return cartItem;
    }

    public async Task<Guid> AddProductToUsersCart(Guid userId, CartItem cartItem)
    {
        var cartItemEntity = new CartItemEntity
        {
            Id = cartItem.Id,
            CartQuantity = cartItem.CartQuantity,
            ProductId = cartItem.ProductId,
            UserId = userId
        };
        
        await dbContext.CartItems.AddAsync(cartItemEntity);
        await dbContext.SaveChangesAsync();
        return cartItemEntity.Id;
    }

    public async Task<Guid?> UpdateQuantity(Guid userId, Guid itemId, int quantity)
    {
        var cartItemEntity = await dbContext.CartItems
            .FirstOrDefaultAsync(ci => ci.Id == itemId && ci.UserId == userId);

        if (cartItemEntity == null)
        {
            return null;
        }

        cartItemEntity.CartQuantity = quantity;
        await dbContext.SaveChangesAsync();
        return cartItemEntity.Id;
    }

    public async Task<Guid?> DeleteItem(Guid userId, Guid itemId)
    {
        int rowsDeleted = await dbContext.CartItems
            .Where(ci => ci.Id == itemId && ci.UserId == userId)
            .ExecuteDeleteAsync();

        if (rowsDeleted == 0)
        {
            return null;
        }

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
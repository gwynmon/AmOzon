using System.ComponentModel.DataAnnotations;

namespace AmOzon.Domain.Models;

public class CartItem
{
    private CartItem(Guid id, int quantity, Guid productId, Guid userId)
    {
        Id = id;
        CartQuantity = quantity;
        ProductId = productId;
        UserId = userId;
    }
    
    public Guid Id { get; }
    public int CartQuantity { get; }
    public Guid ProductId { get; }
    public Guid UserId { get; }

    public static CartItem Create(
        Guid id, int quantity, Guid productId, Guid userId)
    {
        if (quantity <= 0)
        {
            throw new ValidationException("Cart item quantity can not be zero or negative");
        }

        var cartItem = new CartItem(id, quantity, productId, userId);
        
        return cartItem;
    }
}
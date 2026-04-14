using System.ComponentModel.DataAnnotations;
using AmOzon.Domain.Constants;

namespace AmOzon.Domain.Models;

public class OrderItem
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    
    public static OrderItem Create(Guid orderId, Guid productId, int quantity, decimal price)
    {
        if (quantity >= ValidationConstants.MaxStockQuantity)
        {
            throw new ValidationException("Quantity is too large");
        }

        if (price >= ValidationConstants.MaxProductPrice)
        {
            throw new ValidationException("Price is too large");
        }
        
        return new OrderItem
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            ProductId = productId,
            Quantity = quantity,
            Price = price
        };
    }
}
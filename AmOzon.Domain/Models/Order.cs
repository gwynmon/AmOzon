using AmOzon.Contracts.Enums;

namespace AmOzon.Domain.Models;

public class Order
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = nameof(OrderStatuses.Pending);
    
    private readonly List<OrderItem> items = new();
    public IReadOnlyCollection<OrderItem> Items => items.AsReadOnly();
    
    public static Order Create(Guid id, Guid userId, DateTime createdAt, string status, decimal totalAmount)
    {
        return new Order
        {
            Id = id,
            UserId = userId,
            CreatedAt = createdAt,
            Status = status,
            TotalAmount = totalAmount
        };
    }
    
    public void AddItem(OrderItem item)
    {
        items.Add(item);
        TotalAmount += item.Price * item.Quantity;
    }
    
    public void RecalculateTotal()
    {
        TotalAmount = items.Sum(i => i.Price * i.Quantity);
    }
}
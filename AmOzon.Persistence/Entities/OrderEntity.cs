namespace AmOzon.Persistence.Entities;

public class OrderEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; }
    
    public List<OrderItemEntity> Items { get; set; }
}
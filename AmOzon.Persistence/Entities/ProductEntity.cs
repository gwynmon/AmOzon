namespace AmOzon.Persistence.Entities;

public class ProductEntity
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public decimal Price { get; set; }
    public int Amount { get; set; }
    public Guid SellerId { get; set; }
    public Seller? Seller { get; set; }
}

public class Seller
{
}
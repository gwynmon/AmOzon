namespace AmOzon.Persistence.Entities;

public class ProductEntity
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; init; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public Guid SellerId { get; init; }
    public SellerEntity? Seller { get; init; }
    public bool IsDeleted { get; set; }
}
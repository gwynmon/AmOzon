using AmOzon.Domain.Models;

namespace AmOzon.Persistence.Entities;

public class ProductEntity
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public decimal Price { get; init; }
    public int Amount { get; init; }
    public Guid SellerId { get; init; }
    public SellerEntity? Seller { get; init; }
}
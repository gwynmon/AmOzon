namespace AmOzon.Persistence.Entities;

public class SellerEntity
{
    public Guid Id { get; init; }

    public List<ProductEntity> Products { get; init; } = [];
}
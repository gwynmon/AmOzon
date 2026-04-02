namespace AmOzon.Persistence.Entities;

public class SellerEntity
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; } 
    public UserEntity? User { get; init; }
    public List<ProductEntity> Products { get; init; } = [];
}
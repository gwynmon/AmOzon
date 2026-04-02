namespace AmOzon.Persistence.Entities;

public class UserEntity
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public int Age { get; init; }
    public UserCredentialsEntity? UserCredentialsEntity { get; init; }
    public SellerEntity? Seller { get; init; }
    public ICollection<CartItemEntity> ProductsInCart { get; init; } = new List<CartItemEntity>();
}
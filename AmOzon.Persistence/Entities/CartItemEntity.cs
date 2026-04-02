namespace AmOzon.Persistence.Entities;

public class CartItemEntity
{
    public Guid Id { get; init; }
    public int CartQuantity { get; set; }
    public Guid ProductId { get; set; }
    public ProductEntity? Product { get; set; }
    public Guid UserId { get; set; }
    public UserEntity? User { get; set; }
}
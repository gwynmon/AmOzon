namespace AmOzon.Domain.Models;

public class Product
{
    public const int MAX_NAME_LENGTH = 128;
    public Product(Guid id, string name, string description, decimal price, int amount, Guid sellerId)
    {
        Id = id;
        Name = name;
        Description = description;
        CreatedAt = DateTime.UtcNow;
        Price = price;
        Amount = amount;
        SellerId = sellerId;
    }
    
    public Guid Id { get; }
    public string Name { get; } = string.Empty;
    public string Description { get; } = string.Empty;
    public DateTime CreatedAt {get; }
    public decimal Price { get; }
    public int Amount { get; }
    public Guid SellerId { get;}
    public Seller? Seller { get;}

    public static (Product Product, string Error) Create(Guid id, string name, string description, decimal price,
        int amount, Guid sellerId)
    {
        var error = string.Empty;

        if (string.IsNullOrWhiteSpace(name) || name.Length > MAX_NAME_LENGTH)
        {
            error = "Invalid product name";
        }
        // Add More Validation
        
        var product = new Product(id, name, description, price, amount, sellerId);
        
        return (product, error);
    }
}
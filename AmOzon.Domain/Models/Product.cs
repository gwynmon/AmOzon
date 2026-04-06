using System.ComponentModel.DataAnnotations;
using AmOzon.Domain.Constants;

namespace AmOzon.Domain.Models;

public class Product
{
    private Product(Guid id, string name, string description, 
        DateTime createdAt, decimal price, int stockQuantity, 
        Guid sellerId, bool isDeleted)
    {
        Id = id;
        Name = name;
        Description = description;
        CreatedAt = createdAt;
        Price = price;
        StockQuantity = stockQuantity;
        SellerId = sellerId;
        IsDeleted = isDeleted;
    }
    
    public Guid Id { get; }
    public string? Name { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedAt {get; }
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }
    public Guid SellerId { get;}
    public bool IsDeleted { get; private set; }

    public static Product Create(
        Guid id, string name, string description, DateTime createdAt,
        decimal price, int stockQuantity, Guid sellerId, bool isDeleted)
    {
        if (stockQuantity > ValidationConstants.MaxStockQuantity)
        {
            throw new ValidationException("Stock quantity must be less than 1000");
        }
        
        if (price > ValidationConstants.MaxProductPrice)
        {
            throw new ValidationException("Price must be less than 1000");
        }
        
        var product = new Product(id, name, description, createdAt, 
            price, stockQuantity, sellerId, isDeleted);
        
        return product;
    }

    public void Update(string newName, string newDescription, 
        decimal newPrice, int newStockQuantity)
    {
        if (IsDeleted)
        {
            throw new ValidationException("Product cannot be changed if it's marked as deleted");
        }
        
        Name = newName;
        Description = newDescription;
        Price = newPrice;
        StockQuantity = newStockQuantity;
    }
}
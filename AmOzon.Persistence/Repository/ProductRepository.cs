using AmOzon.Domain.Abstractions;
using AmOzon.Domain.Models;
using AmOzon.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AmOzon.Persistence.Repository;

public class ProductRepository : IProductRepository
{
    private readonly AmOzonDbContext _context;
    
    public ProductRepository(AmOzonDbContext dbContext)
    {
        _context = dbContext;
    }
    
    public async Task<List<Product>> GetAll()
    {
        var productEntities = await _context.Products
            .AsNoTracking()
            .ToListAsync();

        var products = productEntities
            .Select(p => Product.Create(p.Id, p.Name, p.Description, p.Price, p.Amount, p.SellerId).Product)
            .ToList();

        return products;
    }

    public async Task<Guid> Create(Product product)
    {
        var productEntity = new ProductEntity
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Amount = product.Amount,
            SellerId = product.SellerId
        };
        
        await _context.Products.AddAsync(productEntity);
        await _context.SaveChangesAsync();
        
        return productEntity.Id;
    }

    public async Task<Guid> Update(Guid id, string name, string description, decimal price, int amount, Guid sellerId)
    {
        await _context.Products
            .Where(p => p.Id == id)
            .ExecuteUpdateAsync<ProductEntity>(s => s
                .SetProperty(p => p.Name, p => name)
                .SetProperty(p => p.Description, p => description)
                .SetProperty(p => p.Price, p => price)
                .SetProperty(p => p.Amount, p => amount)
                .SetProperty(p => p.SellerId, p => sellerId));
        
        await _context.SaveChangesAsync();
        return id;
    }

    public async Task<Guid> Delete(Guid id)
    {
        await _context.Products
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync();
        
        await _context.SaveChangesAsync();
        return id;
    }
}
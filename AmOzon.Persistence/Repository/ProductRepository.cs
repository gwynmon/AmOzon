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
            .Select(p => Product.Create(p.ProductId, p.Name, p.Description, p.Price, p.Amount, p.SellerId).Product)
            .ToList();

        return products;
    }

    public async Task<Guid> Create(Product product)
    {
        var productEntity = new ProductEntity
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Amount = product.Amount,
            SellerId = product.SellerId
        };
        
        await _context.Products.AddAsync(productEntity);
        await _context.SaveChangesAsync();
        
        return productEntity.ProductId;
    }

    public async Task<Guid> Update(Product product)
    {
        await _context.Products
            .Where(p => p.ProductId == product.ProductId)
            .ExecuteUpdateAsync<ProductEntity>(s => s
                .SetProperty(p => p.Name, p => product.Name)
                .SetProperty(p => p.Description, p => product.Description)
                .SetProperty(p => p.Price, p => product.Price)
                .SetProperty(p => p.Amount, p => product.Amount)
                .SetProperty(p => p.SellerId, p => product.SellerId));
        
        await _context.SaveChangesAsync();
        return product.ProductId;
    }

    public async Task<Guid> Delete(Guid id)
    {
        await _context.Products
            .Where(p => p.ProductId == id)
            .ExecuteDeleteAsync();
        
        await _context.SaveChangesAsync();
        return id;
    }
}
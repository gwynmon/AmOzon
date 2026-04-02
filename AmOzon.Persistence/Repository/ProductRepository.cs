using AmOzon.Domain.Abstractions;
using AmOzon.Domain.Models;
using AmOzon.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AmOzon.Persistence.Repository;

public class ProductRepository(AmOzonDbContext dbContext) : IProductRepository
{
    public async Task<List<Product>> GetAll()
    {
        var productEntities = await dbContext.Products
            .AsNoTracking()
            .ToListAsync();

        var products = productEntities
            .Select(p => Product.Create(
                p.Id, 
                p.Name, 
                p.Description, 
                p.CreatedAt, 
                p.Price, 
                p.StockQuantity, 
                p.SellerId, 
                p.IsDeleted
                ))
            .ToList();
        Console.WriteLine(productEntities.Count);
        Console.WriteLine(products.Count);
        return products;
    }

    public async Task<Product?> GetById(Guid id)
    {
        var productEntity = await dbContext.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        if (productEntity == null)
        {
            return null;
        }

        var product = Product.Create(
            productEntity.Id,
            productEntity.Name,
            productEntity.Description,
            productEntity.CreatedAt,
            productEntity.Price,
            productEntity.StockQuantity,
            productEntity.SellerId,
            productEntity.IsDeleted
            );

        return product;
    }

    public async Task<List<Product>> GetBySellerId(Guid id)
    {
        var  productEntities = await dbContext.Products
            .AsNoTracking()
            .ToListAsync();

        var products = productEntities
            .Select(p => Product.Create(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.CreatedAt,
                    p.Price,
                    p.StockQuantity,
                    p.SellerId,
                    p.IsDeleted
                ))
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
            CreatedAt = product.CreatedAt,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            SellerId = product.SellerId,
            IsDeleted = product.IsDeleted
        };
        
        await dbContext.Products.AddAsync(productEntity);
        await dbContext.SaveChangesAsync();
        
        return productEntity.Id;
    }

    public async Task<Guid> Update(Product product)
    {
        await dbContext.Products
            .Where(p => p.Id == product.Id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.Name, product.Name)
                .SetProperty(p => p.Description, product.Description)
                .SetProperty(p => p.Price, product.Price)
                .SetProperty(p => p.StockQuantity, product.StockQuantity));
        
        await dbContext.SaveChangesAsync();
        return product.Id;
    }

    public async Task<Guid> Delete(Guid id)
    {
        await dbContext.Products
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync();
        
        await dbContext.SaveChangesAsync();
        return id;
    }

    public async Task<Guid?> MarkDeleted(Guid id)
    {
        var product = await dbContext.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return null;
        }
        
        product.IsDeleted = true;
        await dbContext.SaveChangesAsync();
        return id;
    }

    public async Task<Guid?> RevokeDeleted(Guid id)
    {
        var product = await dbContext.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return null;
        }
        
        product.IsDeleted = false;
        await dbContext.SaveChangesAsync();
        return id;
    }
}
using AmOzon.Domain.Abstractions;
using AmOzon.Domain.Models;
using AmOzon.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AmOzon.Persistence.Repository;

public class ProductRepository(AmOzonDbContext dbContext) : IProductRepository
{
    private static Product MapToDomain(ProductEntity entity)
    {
        return Product.Create(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.CreatedAt,
            entity.Price,
            entity.StockQuantity,
            entity.SellerId,
            entity.IsDeleted
        );
    }
    public async Task<List<Product>> GetAll()
    {
        var productEntities = await dbContext.Products
            .AsNoTracking()
            .ToListAsync();

        return productEntities
            .Select(MapToDomain)
            .ToList();
    }

    public async Task<Product?> GetById(Guid id)
    {
        var productEntity = await dbContext.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        if (productEntity == null)
        {
            return null;
        }

        return MapToDomain(productEntity);
    }

    public async Task<List<Product>> GetBySellerId(Guid id)
    {
        var productEntities = await dbContext.Products
            .AsNoTracking()
            .ToListAsync();

        return productEntities
            .Select(MapToDomain)
            .ToList();
    }

    public async Task<Guid?> Create(Product product)
    {
        bool sellerExists = await dbContext.Sellers.AnyAsync(s => s.Id == product.SellerId);

        if (!sellerExists)
        {
            return null;
        }
        
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
        var rowsUpdated = await dbContext.Products
            .Where(p => p.Id == product.Id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.Name, product.Name)
                .SetProperty(p => p.Description, product.Description)
                .SetProperty(p => p.Price, product.Price)
                .SetProperty(p => p.StockQuantity, product.StockQuantity));

        if (rowsUpdated == 0)
        {
            return Guid.Empty;
        }
        
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
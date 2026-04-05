using AmOzon.Domain.Abstractions;
using AmOzon.Domain.Models;
using AmOzon.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AmOzon.Persistence.Repository;

public class SellerRepository(AmOzonDbContext dbContext) : ISellerRepository
{
    public async Task<Guid> Create(Seller seller)
    {
        var sellerEntity = new SellerEntity
        {
            Id = seller.Id,
            UserId = seller.UserId,
        };

        await dbContext.Sellers.AddAsync(sellerEntity);
        await dbContext.SaveChangesAsync();

        return sellerEntity.Id;
    }

    public async Task<List<Seller>> GetAll()
    {
        var sellerEntities = await dbContext.Sellers
            .AsNoTracking()
            .ToListAsync();

        var sellers = sellerEntities
            .Select(s => Seller.Create(
                s.Id,
                s.UserId
            ))
            .ToList();

        return sellers;
    }

    public async Task<Seller> GetById(Guid id)
    {
        var sellerEntity = await dbContext.Sellers
            .FirstOrDefaultAsync(s => s.Id == id);

        var seller = Seller.Create(
            sellerEntity.Id,
            sellerEntity.UserId
        );

        return seller;
    }
    
    public async Task<Seller?> GetByUserId(Guid id)
    {
        var sellerEntity = await dbContext.Sellers
            .FirstOrDefaultAsync(s => s.UserId == id);

        if (sellerEntity == null)
        {
            return null;
        }

        var seller = Seller.Create(
            sellerEntity.Id,
            sellerEntity.UserId
        );

        return seller;
    }
}
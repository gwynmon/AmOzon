using AmOzon.Domain.Models;
using AmOzon.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AmOzon.Persistence.Repository;

public class OrderRepository(AmOzonDbContext dbContext) : IOrderRepository
{
    private static Order MapToDomain(OrderEntity entity)
    {
        return Order.Create(
            entity.Id,
            entity.UserId,
            entity.CreatedAt,
            entity.Status,
            entity.TotalAmount
        );
    }

    public async Task<List<Order>> GetByUserId(Guid userId)
    {
        var orderEntity = await dbContext.Orders
            .Include(o => o.Items)
            .Where(o => o.UserId == userId)
            .ToListAsync();

        return orderEntity
            .Select(MapToDomain)
            .ToList();
    }

    public async Task<Order?> GetById(Guid id)
    {
        var orderEntity = await dbContext.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (orderEntity == null)
        {
            return null;
        }
        
        return MapToDomain(orderEntity);
    }

    public async Task<Order> Create(Order order)
    {
        var orderEntity = new OrderEntity
        {
            Id = order.Id,
            UserId = order.UserId,
            CreatedAt = order.CreatedAt,
            TotalAmount = order.TotalAmount,
            Status = order.Status
            
        };
        
        await dbContext.Orders.AddAsync(orderEntity);
        await dbContext.SaveChangesAsync();
        
        return order;
    }
}
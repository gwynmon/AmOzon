using AmOzon.Domain.Models;

namespace AmOzon.Persistence.Repository;

public interface IOrderRepository
{
    Task<List<Order>> GetByUserId(Guid userId);
    Task<Order?> GetById(Guid id);
    Task<Order> Create(Order order);
}
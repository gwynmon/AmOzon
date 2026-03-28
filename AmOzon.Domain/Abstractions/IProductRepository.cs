using AmOzon.Domain.Models;

namespace AmOzon.Domain.Abstractions;

public interface IProductRepository
{
    Task<List<Product>> GetAll();
    Task<Guid> Create(Product product);
    Task<Guid> Update(Guid id, string name, string description, decimal price, int amount, Guid sellerId);
    Task<Guid> Delete(Guid id);
}
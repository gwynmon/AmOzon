using AmOzon.Domain.Models;

namespace AmOzon.Domain.Abstractions;

public interface IProductRepository
{
    Task<List<Product>> GetAll();
    Task<Guid> Create(Product product);
    Task<Guid> Update(Product product);
    Task<Guid> Delete(Guid id);
}
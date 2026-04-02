using AmOzon.Domain.Models;

namespace AmOzon.Domain.Abstractions;

public interface IProductRepository
{
    Task<List<Product>> GetAll();
    Task<Product?> GetById(Guid id);
    Task<List<Product>> GetBySellerId(Guid id);
    Task<Guid> Create(Product product);
    Task<Guid> Update(Product product);
    Task<Guid> Delete(Guid id);
    Task<Guid?> MarkDeleted(Guid id);
    Task<Guid?> RevokeDeleted(Guid id);
}
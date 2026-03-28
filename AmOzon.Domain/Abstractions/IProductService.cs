using AmOzon.Domain.Models;

namespace AmOzon.Application.Services;

public interface IProductService
{
    Task<List<Product>> GetAllProducts();
    Task<Guid> CreateProduct(Product product);
    Task<Guid> UpdateProduct(Guid id, string name, string description, decimal price, int amount, Guid sellerId);
    Task<Guid> DeleteProduct(Guid productId);
}
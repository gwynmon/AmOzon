using AmOzon.Application.Commands;
using AmOzon.Domain.Models;

namespace AmOzon.Application.Abstractions;

public interface IProductService
{
    Task<List<Product>?> GetAllProducts();
    Task<Product?> GetProduct(Guid id);
    Task<List<Product>> GetProductsBySeller(Guid id);
    Task<Guid> CreateProductAsync(CreateProductCommand request);
    Task<Guid> UpdateProduct(UpdateProductCommand command);
    Task<Guid> DeleteProduct(Guid productId);
    Task<Guid> MarkDeleted(Guid id);
    Task<Guid> RevokeDeleted(Guid id);
}
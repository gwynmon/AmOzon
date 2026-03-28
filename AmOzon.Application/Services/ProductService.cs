using AmOzon.Domain.Abstractions;
using AmOzon.Domain.Models;

namespace AmOzon.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    
    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<Product>> GetAllProducts()
    {
        return await _productRepository.GetAll();
    }
    
    public async Task<Guid> CreateProduct(Product product)
    {
        return await _productRepository.Create(product);
    }
    
    public async Task<Guid> UpdateProduct(Guid id, string name, string description, decimal price, int amount, Guid sellerId)
    {
        return await _productRepository.Update( id, name, description, price, amount, sellerId);
    }
    
    public async Task<Guid> DeleteProduct(Guid productId)
    {
        return await _productRepository.Delete(productId);
    }
}
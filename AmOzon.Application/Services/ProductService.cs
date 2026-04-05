using System.ComponentModel.DataAnnotations;
using AmOzon.Application.Abstractions;
using AmOzon.Application.Commands;
using AmOzon.Contracts.Requests;
using AmOzon.Domain.Abstractions;
using AmOzon.Domain.Models;
using Mapster;

namespace AmOzon.Application.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public async Task<List<Product>?> GetAllProducts()
    {
        return await productRepository.GetAll();
    }

    public async Task<Product> GetProduct(Guid id)
    {
        var product = await productRepository.GetById(id);

        if (product == null)
        {
            throw new ApplicationException($"Product with id {id} does not exists");
        }
        
        return product;
    }

    public async Task<List<Product>> GetProductsBySeller(Guid id)
    {
        return await productRepository.GetBySellerId(id);
    }

    public async Task<Guid> CreateProductAsync(ProductsCreateRequest request)
    {
        var command = request.Adapt<CreateProductCommand>();
        
        var product = Product.Create(
            Guid.NewGuid(),
            command.Name,
            command.Description,
            DateTime.UtcNow,
            command.Price,
            command.StockQuantity,
            command.SellerId,
            false
        );
        
        return await productRepository.Create(product);
    }
    
    public async Task<Guid> UpdateProduct(Guid id, ProductsUpdateRequest request)
    {
        var command = request.Adapt<UpdateProductCommand>();
        command = command with { Id = id };
        
        var product = await productRepository.GetById(command.Id);

        if (product == null)
        {
            throw new ApplicationException($"Product with id {command.Id} does not exists");
        }
        
        product.Update(command.Name, command.Description, command.Price, command.StockQuantity);
        
        return await productRepository.Update(product);
    }
    
    public async Task<Guid> DeleteProduct(Guid productId)
    {
        return await productRepository.Delete(productId);
    }

    public async Task<Guid> MarkDeleted(Guid id)
    {
        var productId = await productRepository.MarkDeleted(id);
        return productId ?? throw new ValidationException("Product already marked deleted or does not exists");
    }

    public async Task<Guid> RevokeDeleted(Guid id)
    {
        var productId = await productRepository.RevokeDeleted(id);
        return productId ?? throw new ValidationException("Product already revoked or does not exists");
    }
}
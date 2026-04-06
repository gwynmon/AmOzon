using System.ComponentModel.DataAnnotations;
using AmOzon.Application.Abstractions;
using AmOzon.Application.Commands;
using AmOzon.Domain.Abstractions;
using AmOzon.Domain.Models;

namespace AmOzon.Application.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public async Task<List<Product>?> GetAllProducts()
    {
        return await productRepository.GetAll();
    }

    public async Task<Product?> GetProduct(Guid id)
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

    public async Task<Guid> CreateProductAsync(CreateProductCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Name) || string.IsNullOrWhiteSpace(command.Description))
        {
            throw new ValidationException("Product name and product description are required");
        }

        if (command.Price <= 0 || command.StockQuantity <= 0)
        {
            throw new ValidationException("Price and stock quantity can not be zero or negative");
        }

        if (command.SellerId == Guid.Empty)
        {
            throw new ValidationException($"Seller with id {command.SellerId} does not exists");
        }
        
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
        
        var productId = await productRepository.Create(product);

        if (productId == null)
        {
            throw new ApplicationException($"Seller with id {command.SellerId} does not exists");
        }

        return productId.Value;
    }
    
    public async Task<Guid> UpdateProduct(UpdateProductCommand command)
    {
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

        if (productId == null)
        {
            throw new ValidationException("Product already marked deleted or does not exists");
        }
        
        return productId.Value;
    }

    public async Task<Guid> RevokeDeleted(Guid id)
    {
        var productId = await productRepository.RevokeDeleted(id);
        
        if (productId == null)
        {
            throw new ValidationException("Product already marked revoked or does not exists");
        }
        
        return productId.Value;
    }
}
using AmOzon.API.Contracts;
using AmOzon.Application.Services;
using AmOzon.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace AmOzon.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetAllProducts()
    {
        var products = await _productService.GetAllProducts();

        var response = products.Select(p =>
            new ProductsResponse(p.Id, p.Name, p.Description, p.CreatedAt, p.Price, p.Amount,
                p.SellerId));
        
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateProduct([FromBody] ProductsRequest request)
    {
        var (product, error) = Product.Create(
            Guid.NewGuid(),
            request.Name,
            request.Description,
            request.Price,
            request.Amount,
            request.SellerId
        );

        if (!string.IsNullOrEmpty(error))
        {
            return BadRequest(error);
        }
        
        var productId = await _productService.CreateProduct(product);
        
        return Ok(productId);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Guid>> UpdateProduct(Guid id, [FromBody] ProductsRequest request)
    {
        var productId = await _productService.UpdateProduct(id, request.Name, request.Description, request.Price, request.Amount, request.SellerId);
        
        return Ok(productId);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        return Ok(await _productService.DeleteProduct(id));
    }
}
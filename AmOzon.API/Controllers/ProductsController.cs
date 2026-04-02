using AmOzon.Application.Abstractions;
using AmOzon.Contracts;
using Microsoft.AspNetCore.Mvc;
using Mapster;

namespace AmOzon.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpPost("create")]
    public async Task<ActionResult<Guid>> CreateProduct([FromBody] ProductsCreateRequest request)
    {
        var productId = await productService.CreateProductAsync(request);
        return CreatedAtAction(nameof(CreateProduct), new { id = productId }, productId); 
    }
    
    [HttpGet("get-all")]
    public async Task<ActionResult<List<ProductsResponse>>> GetAllProducts()
    {
        var products = await productService.GetAllProducts();
        var response = products.Adapt<List<ProductsResponse>>();
        return Ok(response);
    }
    
    [HttpGet("get/{id:guid}")]
    public async Task<ActionResult<ProductsResponse>> GetProductById(Guid id)
    {
        var product = await productService.GetProduct(id);
        return Ok(product);
    }

    [HttpGet("get-by-seller/{id:guid}")]
    public async Task<ActionResult<List<ProductsResponse>>> GetProductsBySeller(Guid id)
    {
        var products = await productService.GetProductsBySeller(id);
        var response = products.Adapt<List<ProductsResponse>>();
        return Ok(response);
    }

    [HttpPut("update/{id:guid}")]
    public async Task<ActionResult<Guid>> UpdateProduct(Guid id, [FromBody] ProductsUpdateRequest request)
    {
        var productId = await productService.UpdateProduct(id, request);
        return Ok(productId);
    }

    [HttpPut("mark-deleted/{id:guid}")]
    public async Task<ActionResult<Guid>> MarkDeleted(Guid id)
    {
        var productId = await productService.MarkDeleted(id);
        return Ok(productId);
    }

    [HttpPut("revoke-deleted/{id:guid}")]
    public async Task<ActionResult<Guid>> RevokeDeleted(Guid id)
    {
        var productId = await productService.RevokeDeleted(id);
        return Ok(productId);
    }
    
    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        return Ok(await productService.DeleteProduct(id));
    }
}
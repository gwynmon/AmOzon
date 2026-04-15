using AmOzon.Application.Abstractions;
using AmOzon.Application.Commands;
using AmOzon.Contracts.Requests;
using AmOzon.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Mapster;
using Microsoft.AspNetCore.Authorization;

namespace AmOzon.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpPost("create")]
    [Authorize(Roles = "Seller,Admin")]
    public async Task<ActionResult<Guid>> CreateProduct([FromBody] ProductsCreateRequest request)
    {
        var command = request.Adapt<CreateProductCommand>();
        var productId = await productService.CreateProductAsync(command);
        return CreatedAtAction(nameof(GetProductById), new { id = productId }, productId); 
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
        var response = product.Adapt<ProductsResponse>();
        return Ok(response);
    }

    [HttpGet("get-by-seller/{id:guid}")]
    public async Task<ActionResult<List<ProductsResponse>>> GetProductsBySeller(Guid id)
    {
        var products = await productService.GetProductsBySeller(id);
        var response = products.Adapt<List<ProductsResponse>>();
        return Ok(response);
    }

    [HttpPut("update/{id:guid}")]
    [Authorize(Roles = "Seller,Admin")]
    public async Task<ActionResult<Guid>> UpdateProduct(Guid id, [FromBody] ProductsUpdateRequest request)
    {
        var command = request.Adapt<UpdateProductCommand>();
        command = command with { Id = id };
        var productId = await productService.UpdateProduct(command);
        return Ok(productId);
    }

    [HttpPut("mark-deleted/{id:guid}")]
    [Authorize(Roles = "Seller,Admin")]
    public async Task<ActionResult<Guid>> MarkDeleted(Guid id)
    {
        var productId = await productService.MarkDeleted(id);
        return Ok(productId);
    }

    [HttpPut("revoke-deleted/{id:guid}")]
    [Authorize(Roles = "Seller")]
    public async Task<ActionResult<Guid>> RevokeDeleted(Guid id)
    {
        var productId = await productService.RevokeDeleted(id);
        return Ok(productId);
    }
    
    [HttpDelete("delete/{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        return Ok(await productService.DeleteProduct(id));
    }
}
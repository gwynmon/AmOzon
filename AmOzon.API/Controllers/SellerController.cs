using AmOzon.Application.Abstractions;
using AmOzon.Application.Commands;
using AmOzon.Contracts;
using AmOzon.Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace AmOzon.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SellerController(ISellerService sellerService) : ControllerBase
{
    [HttpPost("create")]
    public async Task<ActionResult<Guid>> CreateProduct([FromBody] SellerRequest request)
    {
        var command = request.Adapt<CreateSellerCommand>();
        var sellerId = await sellerService.CreateSellerAsync(command);
        return CreatedAtAction(nameof(CreateProduct), new { id = sellerId }, sellerId); 
    }

    [HttpGet("get-all")]
    public async Task<ActionResult<List<Seller>>> GetAllSellers()
    {
        var sellers = await sellerService.GetAllSellers();
        return sellers;
    }

    [HttpGet("get/{id:guid}")]
    public async Task<ActionResult<Seller>> GetSeller(Guid id)
    {
        var seller = await sellerService.GetSeller(id);
        return seller;
    }
}
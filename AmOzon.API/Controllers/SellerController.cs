using AmOzon.API.Extensions;
using AmOzon.Application.Abstractions;
using AmOzon.Contracts.Responses;
using AmOzon.Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmOzon.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SellerController(ISellerService sellerService, IAuthService authService) : ControllerBase
{
    [HttpPost("create")]
    [Authorize]
    public async Task<ActionResult> CreateSeller()
    {
        var userId = User.GetUserId();
        
        var sellerId = await sellerService.CreateSellerAsync(userId);
        var newToken = await authService.RefreshRoleTokenAsync(userId);
        
        return Ok(new
        {
            message = "Seller created and token updated.",
            sellerId,
            newToken
        }); 
    }

    [HttpGet("get-all")]
    public async Task<ActionResult<List<SellerResponse>>> GetAllSellers()
    {
        var sellers = await sellerService.GetAllSellers();
        var response = sellers.Adapt<List<SellerResponse>>(); 
        return Ok(response);
    }

    [HttpGet("get/{id:guid}")]
    public async Task<ActionResult<SellerResponse>> GetSeller(Guid id)
    {
        var seller = await sellerService.GetSeller(id);
        var response = seller.Adapt<SellerResponse>(); 
        return Ok(response);
    }
}
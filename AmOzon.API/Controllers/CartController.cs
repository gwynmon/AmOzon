using AmOzon.Application.Abstractions;
using AmOzon.Contracts;
using AmOzon.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace AmOzon.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class CartController(ICartService cartService) : ControllerBase
{
    [HttpPost("add-item")]
    public async Task<ActionResult<Guid>> AddProductToUsersCart(AddProductToUsersCartRequest request)
    {
        return await cartService.AddProductToUsersCart(request);
    }

    [HttpGet("get-items")]
    public async Task<ActionResult<List<CartItem>>> GetUsersCartItems(Guid userId)
    {
        return await cartService.GetUsersCartItems(userId);
    }

    [HttpPut("update-quantity/{itemId:guid}/{quantity:int}")]
    public async Task<ActionResult<Guid>> UpdateQuantity(Guid itemId, int quantity)
    {
        return await cartService.UpdateQuantity(itemId, quantity);
    }

    [HttpDelete("delete-item/{itemId:guid}")]
    public async Task<ActionResult<Guid>> DeleteItem(Guid itemId)
    {
        return await cartService.DeleteItem(itemId);
    }
    
    [HttpDelete("clear/{userId:guid}")]
    public async Task<ActionResult<Guid>> ClearUsersCart(Guid userId)
    {
        return await cartService.ClearUsersCart(userId);
    }
}
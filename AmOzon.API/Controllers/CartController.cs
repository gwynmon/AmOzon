using AmOzon.API.Extensions;
using AmOzon.Application.Abstractions;
using AmOzon.Contracts.Responses;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmOzon.API.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController(ICartService cartService) : ControllerBase
{
    [HttpPost("add-item")]
    public async Task<ActionResult<Guid>> AddProductToUsersCart(Guid productId, int cartQuantity)
    {
        var userId = User.GetUserId();
        var id = await cartService.AddProductToUsersCart(userId, productId, cartQuantity);
        return Ok(id);
    }

    [HttpGet("get-items")]
    public async Task<ActionResult<List<CartItemResponse>>> GetUsersCartItems()
    {
        var userId = User.GetUserId();
        var cart = await cartService.GetUsersCartItems(userId);
        var cartResponse = cart.Adapt<List<CartItemResponse>>();
        return Ok(cartResponse);
    }

    [HttpPut("update-quantity/{itemId:guid}/{quantity:int}")]
    public async Task<ActionResult<Guid>> UpdateQuantity(Guid itemId, int quantity)
    {
        var userId = User.GetUserId();
        var id = await cartService.UpdateQuantity(userId, itemId, quantity);
        return Ok(id);
    }

    [HttpDelete("delete-item/{itemId:guid}")]
    public async Task<ActionResult<Guid>> DeleteItem(Guid itemId)
    {
        var userId = User.GetUserId();
        var id = await cartService.DeleteItem(userId, itemId);
        return Ok(id);
    }
    
    [HttpDelete("clear")]
    public async Task<ActionResult<Guid>> ClearUsersCart()
    {
        var userId = User.GetUserId();
        var cartId = await cartService.ClearUsersCart(userId);
        return Ok(cartId);
    }
}
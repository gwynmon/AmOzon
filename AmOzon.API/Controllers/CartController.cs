using AmOzon.API.Extensions;
using AmOzon.Application.Abstractions;
using AmOzon.Contracts.Responses;
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
        return await cartService.AddProductToUsersCart(userId, productId, cartQuantity);
    }

    [HttpGet("get-items")]
    public async Task<ActionResult<List<CartItemResponse>>> GetUsersCartItems()
    {
        var userId = User.GetUserId();
        var cart = await cartService.GetUsersCartItems(userId);
        return Ok(cart);
    }

    [HttpPut("update-quantity/{itemId:guid}/{quantity:int}")]
    public async Task<ActionResult<Guid>> UpdateQuantity(Guid itemId, int quantity)
    {
        var userId = User.GetUserId();
        return await cartService.UpdateQuantity(userId, itemId, quantity);
    }

    [HttpDelete("delete-item/{itemId:guid}")]
    public async Task<ActionResult<Guid>> DeleteItem(Guid itemId)
    {
        var userId = User.GetUserId();
        return await cartService.DeleteItem(userId, itemId);
    }
    
    [HttpDelete("clear")]
    public async Task<ActionResult<Guid>> ClearUsersCart()
    {
        var userId = User.GetUserId();
        var cartId = await cartService.ClearUsersCart(userId);
        return Ok(cartId);
    }
}
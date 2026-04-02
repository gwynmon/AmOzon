using AmOzon.Contracts;
using AmOzon.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace AmOzon.Application.Abstractions;

public interface ICartService
{
    Task<Guid> AddProductToUsersCart(AddProductToUsersCartRequest request);
    Task<ActionResult<List<CartItem>>> GetUsersCartItems(Guid userId);
    Task<Guid> UpdateQuantity(Guid itemId, int quantity);
    Task<Guid> DeleteItem(Guid itemId);
    Task<Guid> ClearUsersCart(Guid userId);
}
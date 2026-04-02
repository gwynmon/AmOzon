using AmOzon.Application.Commands;
using AmOzon.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace AmOzon.Application.Abstractions;

public interface ISellerService
{
    Task<Guid> CreateSellerAsync(CreateSellerCommand command);
    Task<List<Seller>> GetAllSellers();
    Task<Seller> GetSeller(Guid id);
}
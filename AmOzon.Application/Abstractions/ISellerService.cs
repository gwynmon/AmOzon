using AmOzon.Domain.Models;

namespace AmOzon.Application.Abstractions;

public interface ISellerService
{
    Task<Guid> CreateSellerAsync(Guid userId);
    Task<List<Seller>> GetAllSellers();
    Task<Seller> GetSeller(Guid id);
}
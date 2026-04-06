using AmOzon.Domain.Models;

namespace AmOzon.Domain.Abstractions;

public interface ISellerRepository
{
    Task<Guid> Create(Seller seller);
    Task<List<Seller>> GetAll();
    Task<Seller?> GetById(Guid id);
    Task<Seller?> GetByUserId(Guid id);
}
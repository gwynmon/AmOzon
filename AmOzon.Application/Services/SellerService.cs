using AmOzon.Application.Abstractions;
using AmOzon.Application.Commands;
using AmOzon.Domain.Abstractions;
using AmOzon.Domain.Models;

namespace AmOzon.Application.Services;

public class SellerService(ISellerRepository sellerRepository) : ISellerService
{
    public async Task<Guid> CreateSellerAsync(CreateSellerCommand command)
    {
        var seller = Seller.Create(
            Guid.NewGuid(),
            command.UserId
        );

        var sellerId = await sellerRepository.Create(seller);
        
        return sellerId;
    }

    public async Task<List<Seller>> GetAllSellers()
    {
        return await sellerRepository.GetAll();
    }

    public async Task<Seller> GetSeller(Guid id)
    {
        return await sellerRepository.GetById(id);
    }
}
using System.ComponentModel.DataAnnotations;

namespace AmOzon.Domain.Models;

public class Seller
{
    private Seller(Guid id, Guid userId)
    {
        Id = id;
        UserId = userId;
    }
    
    public Guid Id { get; }
    public Guid UserId { get; } 
    public static Seller Create(Guid id, Guid userId)
    {
        if (id == Guid.Empty)
        {
            throw new ValidationException("Invalid seller Guid.");
        }
        
        if (userId == Guid.Empty)
        {
            throw new ValidationException("Invalid user Guid.");
        }

        var seller = new Seller(id, userId);
        
        return seller;
    }
}
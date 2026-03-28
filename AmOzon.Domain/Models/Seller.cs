namespace AmOzon.Domain.Models;

public class Seller
{
    public Seller(Guid id)
    {
        Id = id;
    }
    
    public Guid Id { get; }

    public static (Seller seller, string error) Create(Guid id)
    {
        string error = string.Empty;
        
        var seller = new Seller(id);
        
        return (seller, error);
    }
}
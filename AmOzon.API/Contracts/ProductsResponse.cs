namespace AmOzon.API.Contracts;

public record ProductsResponse(
    Guid Id,
    string Name,
    string Description,
    DateTime DateCreated,
    decimal Price,
    int Amount,
    Guid SellerId
);
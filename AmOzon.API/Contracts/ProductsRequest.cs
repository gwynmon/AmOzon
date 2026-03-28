namespace AmOzon.API.Contracts;

public record ProductsRequest(
    Guid Id,
    string Name,
    string Description,
    DateTime DateCreated,
    decimal Price,
    int Amount,
    Guid SellerId
);
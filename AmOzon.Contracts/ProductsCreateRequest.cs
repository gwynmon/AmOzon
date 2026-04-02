namespace AmOzon.Contracts;

public record ProductsCreateRequest(
    Guid? Id,
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    Guid? SellerId
);
namespace AmOzon.Contracts.Requests;

public record ProductsCreateRequest(
    Guid? Id,
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    Guid? SellerId
);
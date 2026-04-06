namespace AmOzon.Contracts.Requests;

public record ProductsCreateRequest(
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    Guid SellerId
);
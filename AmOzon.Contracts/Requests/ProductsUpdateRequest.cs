namespace AmOzon.Contracts.Requests;

public record ProductsUpdateRequest(
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    Guid? SellerId
);
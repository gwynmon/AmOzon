namespace AmOzon.Contracts.Responses;

public record ProductsResponse(
    Guid Id,
    string Name,
    string Description,
    DateTime CreatedAt,
    decimal Price,
    int StockQuantity,
    Guid SellerId,
    bool IsDeleted
);
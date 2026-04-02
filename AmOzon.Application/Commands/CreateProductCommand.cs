namespace AmOzon.Application.Commands;

public record CreateProductCommand(
    string Name, 
    string Description, 
    decimal Price, 
    int StockQuantity, 
    Guid SellerId
    );
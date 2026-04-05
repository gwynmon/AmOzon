namespace AmOzon.Contracts.Responses;

public record CartItemResponse
(
    Guid Id,
    int CartQuantity,
    Guid ProductId,
    Guid UserId
);
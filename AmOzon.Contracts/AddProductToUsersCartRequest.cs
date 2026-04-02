namespace AmOzon.Contracts;

public record AddProductToUsersCartRequest
(
    int CartQuantity,
    Guid UserId,
    Guid ProductId
);
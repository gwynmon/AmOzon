namespace AmOzon.Contracts.Responses;

public record UserResponse(
    Guid Id,
    string Name,
    int Age,
    string Email,
    bool IsSeller
);
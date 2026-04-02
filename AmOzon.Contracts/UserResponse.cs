namespace AmOzon.Contracts;

public record UserResponse(
    Guid Id,
    string Name,
    int Age,
    string Email,
    string Password
);
namespace AmOzon.Contracts.Requests;

public record RegisterRequest(
    string Name,
    int Age,
    string Email,
    string Password
);
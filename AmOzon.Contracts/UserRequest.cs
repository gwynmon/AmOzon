namespace AmOzon.Contracts;

public record UserRequest(
    string Name,
    int Age,
    string Email,
    string PasswordHash
);

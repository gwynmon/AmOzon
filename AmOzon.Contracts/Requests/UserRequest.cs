namespace AmOzon.Contracts.Requests;

public record UserRequest(
    string Name,
    int Age,
    string Email,
    string PasswordHash
);

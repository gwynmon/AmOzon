namespace AmOzon.Application.Commands;

public record CreateUserCommand (
    string  Name,
    int Age,
    string Email,
    string Password
    );
namespace AmOzon.Contracts.Requests;

public record LoginRequest(
    string Email, 
    string Password
);
namespace AmOzon.Contracts.Responses;

public record AuthResponse(
    string Token, 
    string Email, 
    Guid UserId
);
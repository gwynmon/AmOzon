namespace AmOzon.Contracts.Responses;

public record UserProfileResponse (
    Guid Id,
    string Name,
    string Email,
    int Age,
    bool IsSeller
);
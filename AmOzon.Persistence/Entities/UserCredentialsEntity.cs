using Microsoft.AspNetCore.Identity;

namespace AmOzon.Persistence.Entities;

public class UserCredentialsEntity : IdentityUser<Guid>
{
    public Guid UserId { get; set; }
    public UserEntity? User { get; set; }
}
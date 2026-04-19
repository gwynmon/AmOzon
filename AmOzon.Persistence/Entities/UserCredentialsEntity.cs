using Microsoft.AspNetCore.Identity;

namespace AmOzon.Persistence.Entities;

public class UserCredentialsEntity : IdentityUser<Guid>
{
    public UserEntity? User { get; set; }
}
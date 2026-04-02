namespace AmOzon.Persistence.Entities;

public class UserCredentialsEntity
{
    public Guid UserId { get; set; }
    public UserEntity? User { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
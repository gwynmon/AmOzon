using AmOzon.Application.Commands;
using AmOzon.Domain.Models;

namespace AmOzon.Application.Abstractions;

public interface IUserService
{
    Task<Guid> CreateUserAsync(CreateUserCommand command);
    Task<List<User>> GetAllUsers();
    Task<User?> GetUser(Guid id);
}
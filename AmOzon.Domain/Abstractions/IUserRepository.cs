using AmOzon.Contracts.Requests;
using AmOzon.Domain.Models;

namespace AmOzon.Domain.Abstractions;

public interface IUserRepository
{
    Task<Guid> Create(User user);
    Task<List<User>> GetAll();
    Task<User> GetById(Guid id);
    Task<User?> GetByEmail(string email);
}
using AmOzon.Application.Abstractions;
using AmOzon.Application.Commands;
using AmOzon.Domain.Abstractions;
using AmOzon.Domain.Models;
using AmOzon.Persistence.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AmOzon.Application.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<Guid> CreateUserAsync(CreateUserCommand command)
    {
        var user = User.Create(
            Guid.NewGuid(),
            command.Name,
            command.Age,
            command.Email,
            command.PasswordHash
        );

        var userId = await userRepository.Create(user!);
        
        return userId;
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await userRepository.GetAll();
    }

    public async Task<User> GetUser(Guid id)
    {
        return await userRepository.GetById(id);
    }
}
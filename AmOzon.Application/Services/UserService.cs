using System.ComponentModel.DataAnnotations;
using AmOzon.Application.Abstractions;
using AmOzon.Application.Commands;
using AmOzon.Domain.Abstractions;
using AmOzon.Domain.Models;

namespace AmOzon.Application.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<Guid> CreateUserAsync(CreateUserCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
        {
            throw new ValidationException("User name is required");
        }

        if (command.Age <= 0 )
        {
            throw new ValidationException("Age can not be zero or negative");
        }

        if (string.IsNullOrWhiteSpace(command.Email) || string.IsNullOrWhiteSpace(command.Password))
        {
            throw new ValidationException("Email and password are required");
        }
        
        var user = User.Create(
            Guid.NewGuid(),
            command.Name,
            command.Age,
            command.Email
        );

        var userId = await userRepository.Create(user!);
        
        return userId;
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await userRepository.GetAll();
    }

    public async Task<User?> GetUser(Guid id)
    {
        return await userRepository.GetById(id);
    }
}
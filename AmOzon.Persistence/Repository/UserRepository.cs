using AmOzon.Contracts.Requests;
using AmOzon.Domain.Abstractions;
using AmOzon.Domain.Models;
using AmOzon.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AmOzon.Persistence.Repository;

public class UserRepository(AmOzonDbContext dbContext) : IUserRepository
{
    public async Task<Guid> Create(User user)
    {
        var userEntity = new UserEntity
        {
            Id = user.Id,
            Name = user.Name,
            Age = user.Age,
        };

        var userCredentialsEntity = new UserCredentialsEntity()
        {
            UserId = user.Id,
            Email = user.Email,
            Password = user.Password
        };

        await dbContext.Users.AddAsync(userEntity);
        await dbContext.UserCredentials.AddAsync(userCredentialsEntity);
        await dbContext.SaveChangesAsync();

        return userEntity.Id;
    }

    public async Task<List<User>> GetAll()
    {
        var userEntities = await dbContext.Users
            .AsNoTracking()
            .Include(u => u.UserCredentialsEntity) // Подгружаем credentials
            .ToListAsync();
        
        var users = userEntities.Select(u => User.Create(
                u.Id,
                u.Name,
                u.Age,
                u.UserCredentialsEntity.Email,
                u.UserCredentialsEntity.Password)
            )
                .ToList();
        
        return users;
    }

    public async Task<User> GetById(Guid id)
    {
        var userEntity = await dbContext.Users
            .Include(u => u.UserCredentialsEntity)
            .FirstOrDefaultAsync(u => u.Id == id);

        var user = User.Create(
            userEntity.Id,
            userEntity.Name,
            userEntity.Age,
            userEntity.UserCredentialsEntity.Email,
            userEntity.UserCredentialsEntity.Password
        );
        
        return user;
    }

    public async Task<User?> GetByEmail(string email)
    {
        var userEntity = await dbContext.Users
            .Include(u => u.UserCredentialsEntity)
            .FirstOrDefaultAsync(u => u.UserCredentialsEntity.Email == email);

        var user = User.Create(
            userEntity.Id,
            userEntity.Name,
            userEntity.Age,
            userEntity.UserCredentialsEntity.Email,
            userEntity.UserCredentialsEntity.Password
        );
        
        return user;
    }
}
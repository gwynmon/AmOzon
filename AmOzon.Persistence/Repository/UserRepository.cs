using AmOzon.Domain.Abstractions;
using AmOzon.Domain.Models;
using AmOzon.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AmOzon.Persistence.Repository;

public class UserRepository(AmOzonDbContext dbContext) : IUserRepository
{
    private static User MapToDomain(UserEntity entity)
    {
        var credentials = entity.UserCredentialsEntity!;
        
        return User.Create(
            entity.Id,
            entity.Name,
            entity.Age,
            credentials.Email
        );
    }
    
    public async Task<Guid> Create(User user)
    {
        var userEntity = new UserEntity
        {
            Id = user.Id,
            Name = user.Name,
            Age = user.Age,
        };

        await dbContext.Users.AddAsync(userEntity);
        await dbContext.SaveChangesAsync();

        return userEntity.Id;
    }

    public async Task<List<User>> GetAll()
    {
        var userEntities = await dbContext.Users
            .AsNoTracking()
            .Include(u => u.UserCredentialsEntity) // Подгружаем credentials
            .ToListAsync();
        
        return userEntities.Select(MapToDomain).ToList();
    }

    public async Task<User?> GetById(Guid id)
    {
        var userEntity = await dbContext.Users
            .Include(u => u.UserCredentialsEntity)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (userEntity == null)
        {
            return null;
        }

        return MapToDomain(userEntity);
    }

    public async Task<User?> GetByEmail(string email)
    {
        var userEntity = await dbContext.Users
            .Include(u => u.UserCredentialsEntity)
            .FirstOrDefaultAsync(u => u.UserCredentialsEntity.Email == email);

        if (userEntity == null)
        {
            return null;
        }

        return MapToDomain(userEntity);
    }

    public async Task DeleteByIdAsync(Guid userId)
    {
        await dbContext.Users
            .Where(u => u.Id == userId)
            .ExecuteDeleteAsync<UserEntity>();
        
        await dbContext.SaveChangesAsync();
    }
}
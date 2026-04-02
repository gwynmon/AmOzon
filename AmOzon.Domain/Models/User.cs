using System.ComponentModel.DataAnnotations;

namespace AmOzon.Domain.Models;

public class User
{
    private User(Guid id, string name, int age, string email, string password)
    {
        Id = id;
        Name = name;
        Age = age;
        Email = email;
        Password = password;
    }

    public Guid Id { get; }
    public string Name { get; }
    public int Age { get; }
    public string Email { get; }
    public string Password { get; }

    public static User Create(Guid id, string name, int age, string email, string passwordHash)
    {
        if (id == Guid.Empty)
        {
            throw new ValidationException("Invalid user Guid.");
        }
        
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ValidationException("Invalid user name.");
        }
        
        if (age < 0)
        {
            throw new ValidationException("Invalid user age.");
        }
        
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
        {
            throw new ValidationException("Invalid user email.");
        }   

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ValidationException("Invalid user password hash.");
        }
        
        var user = new User(id, name, age,  email, passwordHash);
        
        return user;
    }
}
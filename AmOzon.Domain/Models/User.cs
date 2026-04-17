using System.ComponentModel.DataAnnotations;

namespace AmOzon.Domain.Models;

public class User
{
    private User(Guid id, string name, int age, string email)
    {
        Id = id;
        Name = name;
        Age = age;
        Email = email;
    }

    public Guid Id { get; }
    public string Name { get; }
    public int Age { get; }
    public string Email { get; }

    public static User Create(Guid id, string name, int age, string email)
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
        
        var user = new User(id, name, age,  email);
        
        return user;
    }
}
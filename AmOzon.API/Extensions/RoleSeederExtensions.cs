using Microsoft.AspNetCore.Identity;
using AmOzon.Persistence.Entities;

namespace AmOzon.API.Extensions;

public static class RoleSeederExtensions
{
    public static async Task SeedRoles(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        
        foreach (var role in new[] { "User", "Seller", "Admin" })
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role) 
                    { NormalizedName = role.ToUpper() });
                Console.WriteLine($"✅ Role '{role}' created");
            }
        }
    }
}
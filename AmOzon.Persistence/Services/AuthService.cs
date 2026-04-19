using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AmOzon.Application.Abstractions;
using AmOzon.Application.Settings;
using AmOzon.Contracts.Requests;
using AmOzon.Contracts.Responses;
using AmOzon.Domain.Abstractions;
using AmOzon.Domain.Models;
using AmOzon.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AmOzon.Persistence.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ISellerRepository _sellerRepository;
    private readonly UserManager<UserCredentialsEntity> _userManager;
    private readonly JwtSettings _jwtSettings;

    
    public AuthService(
        IUserRepository userRepository, 
        ISellerRepository sellerRepository,
        IOptions<JwtSettings> jwtSettings,
        UserManager<UserCredentialsEntity> userManager)
    {
        _userRepository = userRepository;
        _sellerRepository = sellerRepository;
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
    }
    
    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var credentials = await _userManager.FindByEmailAsync(request.Email);

        if (credentials == null)
        {
            return null;
        }

        if (await _userManager.IsLockedOutAsync(credentials))
        {
            return null;
        }
         
        var isValidPassword = await _userManager.CheckPasswordAsync(credentials, request.Password);
        
        if (!isValidPassword)
        {
            await _userManager.AccessFailedAsync(credentials);
            return null;
        }
        
        await _userManager.ResetAccessFailedCountAsync(credentials);
        
        var user = await _userRepository.GetById(credentials.Id);
        if (user == null)
        {
            return null;
        }
        
        var token = await GenerateJwtToken(credentials, user);
        return new AuthResponse(token, credentials.Email, user.Id);
    }
    
    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        var user = User.Create(
            id: Guid.NewGuid(),
            name: request.Name,
            age: request.Age,
            email: request.Email
        );
    
        await _userRepository.Create(user);
        
        var credentials = new UserCredentialsEntity
        {
            Id = user.Id,
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = false
        };

        var identityResult = await _userManager.CreateAsync(credentials, request.Password);
        if (!identityResult.Succeeded)
        {
            await _userRepository.DeleteByIdAsync(user.Id);
            var errors = string.Join("; ", identityResult.Errors.Select(e => e.Description));
            throw new ApplicationException($"Registration failed: {errors}");
        }
        
        await _userManager.AddToRoleAsync(credentials, "User");
        
        var token = await GenerateJwtToken(credentials, user);
        return new AuthResponse(token, credentials.Email, user.Id);
    }
    
    public async Task<string> RefreshRoleTokenAsync(Guid userId)
    {
        var user = await _userRepository.GetById(userId);
        
        if (user == null)
        {
            throw new Exception("User not found");
        }
        
        return await GenerateJwtToken(user);
    }
    
    private async Task<string> GenerateJwtToken(UserCredentialsEntity credentials, User user)
    {
        var roles = await _userManager.GetRolesAsync(credentials);
        var primaryRole = roles.FirstOrDefault() ?? "User";
        
        var seller = await _sellerRepository.GetByUserId(user.Id);
        if (seller != null && !roles.Contains("Seller"))
        {
            await _userManager.AddToRoleAsync(credentials, "Seller");
            primaryRole = "Seller";
        }
        
        return CreateToken(user.Id, credentials.Email, user.Name, primaryRole, roles);
    }
    
    private async Task<string> GenerateJwtToken(User user)
    {
        var credentials = await _userManager.FindByIdAsync(user.Id.ToString());
        if (credentials == null)
            throw new InvalidOperationException("Credentials not found");
        
        return await GenerateJwtToken(credentials, user);
    }

    private string CreateToken(Guid userId, string email, string name, string primaryRole, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, name),
            new Claim(ClaimTypes.Role, primaryRole)
        };
        
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
            signingCredentials: creds
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AmOzon.Application.Abstractions;
using AmOzon.Application.Settings;
using AmOzon.Contracts.Enums;
using AmOzon.Contracts.Requests;
using AmOzon.Contracts.Responses;
using AmOzon.Domain.Abstractions;
using AmOzon.Domain.Models;
using Microsoft.IdentityModel.Tokens;

namespace AmOzon.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtSettings _jwtSettings;
    private readonly ISellerRepository _sellerRepository;
    
    public AuthService(IUserRepository userRepository, JwtSettings jwtSettings, ISellerRepository sellerRepository)
    {
        _userRepository = userRepository;
        _jwtSettings = jwtSettings;
        _sellerRepository = sellerRepository;
    }
    
    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmail(request.Email);

        if (user == null)
        {
            throw new ApplicationException("User not found");
        }

        if (user.Password != request.Password)
        {
            throw new ApplicationException("Passwords do not match");
        }
        
        string token = await GenerateJwtToken(user);
        return new AuthResponse(token, user.Email, user.Id);
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
    
    private async Task<string> GenerateJwtToken(User user)
    {
        var role = UserRoles.User;
        var seller = await _sellerRepository.GetByUserId(user.Id);
        
        if (seller != null)
        {
            role = UserRoles.Seller;
        }

        // For debugging purpose. Yes, I know that it's bad
        if (user.Name == "admin")
        {
            role = UserRoles.Admin;
        }
        
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using deLuca.InventoryManager.Application.DTOs;
using Microsoft.Extensions.Configuration;

namespace deLuca.InventoryManager.Application.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;

    public AuthService(IConfiguration config)
    {
        _config = config;
    }

    public string? Authenticate(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var adminEmail = _config["AdminConfig:Email"];
        var adminPassword = _config["AdminConfig:Password"];

        if (request.Email != adminEmail || request.Password != adminPassword)
            return null;

        return GenerateJwtToken(request.Email);
    }

    private string GenerateJwtToken(string email)
    {
        var jwtKey = _config["Jwt:Key"] ?? throw new Exception("JWT Key not found");
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, email),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

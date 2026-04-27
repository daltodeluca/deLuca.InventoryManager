using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using deLuca.InventoryManager.Api.DTOs;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var auth = app.MapGroup("/api/auth");

        auth.MapPost("/login", (LoginRequest req, IConfiguration config, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("AuthEndpoints");
            var adminEmail = config["AdminConfig:Email"];
            var adminPassword = config["AdminConfig:Password"];

            if (req.Email == adminEmail && req.Password == adminPassword)
            {
                var jwtKey = config["Jwt:Key"];
                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!));
                var claims = new[] { new Claim(ClaimTypes.Name, req.Email), new Claim(ClaimTypes.Role, "Admin") };

                var token = new JwtSecurityToken(
                    issuer: config["Jwt:Issuer"],
                    audience: config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(2),
                    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );
                return Results.Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            logger.LogWarning("Failed login attempt for {Email}", req.Email);
            return Results.Unauthorized();
        });
    }
}
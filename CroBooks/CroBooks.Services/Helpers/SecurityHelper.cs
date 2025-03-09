using CroBooks.Services.Models.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CroBooks.Services.Helpers;

public static class SecurityHelper
{
    public static string CreateToken(string id, string username, string firstName, string lastName, string role, AppSecuritySettingsOptions options)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, id),
            new(ClaimTypes.Name, username),
            new(ClaimTypes.GivenName, $"{firstName} {lastName}"),
            new(ClaimTypes.Role, role)
        };
        //claims.AddRange(authClaims.Select(authClaim => new Claim(ClaimType.DefaultClaimAuthType, authClaim)));

        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key));

        var cred = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha512);

        var token = GenerateToken(claims, cred, DateTime.Now.AddDays(1), options);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    public static string CreatePasswordHash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public static bool ValidatePassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }

    private static JwtSecurityToken GenerateToken(List<Claim> claims,
        SigningCredentials cred,
        DateTime expires,
        AppSecuritySettingsOptions options)
    {
        if (!string.IsNullOrEmpty(options.Issuer) && !string.IsNullOrEmpty(options.Audience))
            return GenerateToken(claims, cred, expires, options.Issuer, options.Audience);

        return GenerateToken(claims, cred, expires);
    }

    private static JwtSecurityToken GenerateToken(List<Claim> claims,
        SigningCredentials cred,
        DateTime expires)
    {
        return new JwtSecurityToken(
            claims: claims,
            expires: expires,
            signingCredentials: cred
        );
    }

    private static JwtSecurityToken GenerateToken(List<Claim> claims, 
        SigningCredentials cred, 
        DateTime expires, 
        string issuer,
        string audience)
    {
        return new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: cred
        );
    }
}
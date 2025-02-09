using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CroBooks.Services.Helpers;

public static class SecurityHelper
{
    public static string CreateToken(string id, string username, string firstName, string lastName, string role, string key)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, id),
            new(ClaimTypes.Name, username),
            new(ClaimTypes.GivenName, $"{firstName} {lastName}"),
            new(ClaimTypes.Role, role)
        };
        //claims.AddRange(authClaims.Select(authClaim => new Claim(ClaimType.DefaultClaimAuthType, authClaim)));

        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        var cred = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: cred
        );

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
}
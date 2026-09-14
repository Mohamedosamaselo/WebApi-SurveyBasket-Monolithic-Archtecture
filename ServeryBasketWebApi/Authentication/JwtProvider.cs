using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SurveyBasketWebApi.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SurveyBasketWebApi.Authentication;

public class JwtProvider(IOptions<JwtOptions> jwtOptions) : IJwtProvider
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public async Task<(string Token, int ExpiresIn)> GenerateTokenAsync(ApplicationUser user)
    {
        //1- set claims
        Claim[] claims = [
            new (JwtRegisteredClaimNames.Sub, user.Id),
            new (JwtRegisteredClaimNames.Email, user.Email!),
            new (JwtRegisteredClaimNames.GivenName, user.FirstName),
            new (JwtRegisteredClaimNames.FamilyName, user.LastName),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ];

        //2- set SecurityKey and SigningCredentials
        var symetricKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtOptions.Key)); // Replace with your secret key

        var signingCredentials = new SigningCredentials(symetricKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);

        ////3- set token expiration
        //var expireIn = _jwtOptions.ExpiryMinutes; // in minutes
        //var expirationDate = DateTime.UtcNow.AddMinutes(expireIn * 60);

        //4- create token
        var token = new JwtSecurityToken(
           issuer: _jwtOptions.Issuer,
           audience: _jwtOptions.Audience,
           claims: claims,
           signingCredentials: signingCredentials,
           expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes * 60)
            );

        return (Token: new JwtSecurityTokenHandler().WriteToken(token), ExpiresIn: _jwtOptions.ExpiryMinutes * 60);
    }
}
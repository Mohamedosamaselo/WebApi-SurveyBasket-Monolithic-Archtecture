using Microsoft.IdentityModel.Tokens;
using SurveyBasketWebApi.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SurveyBasketWebApi.Authentication;

public class JwtProvider : IJwtProvider
{
    public async Task<(string Token, int ExpiresIn)> GenerateTokenAsync(ApplicationUser user)
    {
        // set claims
        Claim[] claims = [
            new (JwtRegisteredClaimNames.Sub, user.Id),
            new (JwtRegisteredClaimNames.Email, user.Email!),
            new (JwtRegisteredClaimNames.GivenName, user.FirstName),
            new (JwtRegisteredClaimNames.FamilyName, user.LastName),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ];

        // set SecurityKey and SigningCredentials
        var symetricKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("QayFdiBLLPPJP3KdUxmorUE4U5jMmopkjmZYx3L2wn8")); // Replace with your secret key

        var signingCredentials = new SigningCredentials(symetricKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);

        // set token expiration
        var expireIn = 30; // in minutes
        var expirationDate = DateTime.UtcNow.AddMinutes(expireIn * 60);

        // create token
        var token = new JwtSecurityToken(
           issuer: "SurveyBasket",
           audience: "SurveyBasketUsers",
           claims: claims,
           signingCredentials: signingCredentials,
           expires: expirationDate
            );

        return (Token: new JwtSecurityTokenHandler().WriteToken(token), ExpiresIn: expireIn);
    }
}
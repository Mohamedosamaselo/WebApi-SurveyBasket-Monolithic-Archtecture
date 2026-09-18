using Microsoft.AspNetCore.Identity;
using SurveyBasketWebApi.Authentication;
using SurveyBasketWebApi.Contracts.Dtos.Authentication;
using SurveyBasketWebApi.Entities;
using System.Security.Cryptography;

namespace SurveyBasketWebApi.Services;

public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider)
    : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    private readonly int _refreshTokenExpiryDays = 14;

    public async Task<AuthReponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        // check if user exists
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return null!;

        // check if password is correct
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);

        if (isPasswordValid == false)
            return null;

        // generate token and expiresIn
        var (Token, ExpiresIn) = await _jwtProvider.GenerateTokenAsync(user);

        /// refresh Token
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        var refreshToken = GenerateRefreshToken();
        // save refresh token in DB (owned collection)
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiration, // example: 7 days validity
            CreatedOn = DateTime.UtcNow
        });

        // update UsersTable
        await _userManager.UpdateAsync(user);

        // return AuthResponse object with user details and token

        return new AuthReponse(user.Id,
                               user.FirstName,
                               user.LastName,
                               user.Email,
                               Token,
                               ExpiresIn,
                               refreshToken,
                               refreshTokenExpiration
                               );
    }

    private static string GenerateRefreshToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(randomBytes);
    }
}
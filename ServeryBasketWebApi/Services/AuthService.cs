using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration.UserSecrets;
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

        // generate JwtToken and expiresIn
        var (Token, ExpiresIn) = await _jwtProvider.GenerateTokenAsync(user);

        /// Generate refreshToken and expiryDateOfRefreshToken
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

    public async Task<AuthReponse?> GetRefreshTokenAsync(string token, string refresfToken, CancellationToken cancellationToken = default)
    {
        //1- get userId from validateToken Method
        var userId = _jwtProvider.ValidateToken(token);
        if (userId is null) return null;

        //2- check on User in Database
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return null;
        //3- Get RefreshToken of user that match with RefreshToken Paramter
        var userRefreshToken = user.RefreshTokens.FirstOrDefault(x => x.Token == refresfToken && x.IsActive);
        if (userRefreshToken is null) return null;

        //4- Revoke userRefreshToken
        userRefreshToken.RevokedOn = DateTime.UtcNow;

        //5- generate New JwtToken
        var (newToken, ExpiresIn) = await _jwtProvider.GenerateTokenAsync(user);

        //6- Generate expiryDateOfRefreshToken
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);
        //7- Generate RefreshToken
        var newRefreshToken = GenerateRefreshToken();

        //8- save refresh token in DB (owned collection)
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = refreshTokenExpiration, // example: 7 days validity
            CreatedOn = DateTime.UtcNow
        });

        //9- update UsersTable
        await _userManager.UpdateAsync(user);

        //10- Return AuthResponse
        return new AuthReponse(user.Id,
                             user.FirstName,
                             user.LastName,
                             user.Email,
                             newToken,
                             ExpiresIn,
                             newRefreshToken,
                             refreshTokenExpiration
                             );
    }

    #region Helpers Method

    private static string GenerateRefreshToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(randomBytes);
    }

    #endregion Helpers Method
}
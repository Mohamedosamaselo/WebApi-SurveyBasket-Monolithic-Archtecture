using Microsoft.AspNetCore.Identity;
using SurveyBasketWebApi.Authentication;
using SurveyBasketWebApi.Contracts.Dtos.Authentication;
using SurveyBasketWebApi.Entities;

namespace SurveyBasketWebApi.Services;

public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task<AuthReponse?> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
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

        // return AuthResponse object with user details and token

        return new AuthReponse(user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            Token,
            ExpiresIn
           );
    }
}
using Microsoft.AspNetCore.Mvc;
using SurveyBasketWebApi.Contracts.Dtos.Authentication;

namespace SurveyBasketWebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    // login Endpoint
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(loginRequest request, CancellationToken cancellationToken = default)
    {
        var AuthResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

        return AuthResult is null ? BadRequest("Invalid Email/Password") : Ok(AuthResult);
    }

    [HttpPost("refreshToken")]
    public async Task<IActionResult> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var AuthResult = await _authService.GetRefreshTokenAsync(request.token, request.refreshToken, cancellationToken);

        return AuthResult is null ? BadRequest("Invalid token ") : Ok(AuthResult);
    }
}
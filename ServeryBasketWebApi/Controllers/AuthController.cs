using Microsoft.AspNetCore.Mvc;
using SurveyBasketWebApi.Contracts.Dtos.Authentication;

namespace SurveyBasketWebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    // login Endpoint
    [HttpPost("")]
    public async Task<IActionResult> Login(loginRequest request,
        CancellationToken cancellationToken = default)
    {
        var AuthResult = await _authService.LoginAsync(request.Email, request.Password, cancellationToken);

        return AuthResult is null ? BadRequest("Invalid Email/Password") : Ok(AuthResult);
    }
}
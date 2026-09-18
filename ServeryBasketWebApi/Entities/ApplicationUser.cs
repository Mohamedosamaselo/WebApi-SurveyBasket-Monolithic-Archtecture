using Microsoft.AspNetCore.Identity;

namespace SurveyBasketWebApi.Entities;

public sealed class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    // navigational properties[ every user has list of RefreshTokens ]
    public List<RefreshToken> RefreshTokens { get; set; } = [];
}
using System.ComponentModel.DataAnnotations;

namespace SurveyBasketWebApi.Authentication;

public class JwtOptions
{
    public static string SectionName => "Jwt";// the section name in appsettings.json for JWT configuration

    [Required(ErrorMessage = "JWT Key is required.")]
    public string Key { get; init; } = string.Empty;

    [Required(ErrorMessage = "JWT Issuer is required.")]
    public string Issuer { get; init; } = string.Empty;

    [Required(ErrorMessage = "JWT Audience is required.")]
    public string Audience { get; init; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "JWT Expiry Minutes must be a positive integer.")]
    public int ExpiryMinutes { get; init; }
}
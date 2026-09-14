namespace SurveyBasketWebApi.Entities;

[Owned]
public class refreshToken
{
    public string Token { get; set; }

    public DateTime ExpirOn { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? RevokedOn { get; set; }

    public bool IsExpired => DateTime.UtcNow >= ExpirOn;
    public bool IsActive => RevokedOn is null && !IsExpired;
}
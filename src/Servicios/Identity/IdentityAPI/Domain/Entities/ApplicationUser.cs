namespace Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [JsonIgnore]
    public List<RefreshToken> RefreshTokens { get; set; }

}
namespace Application.Common.DTOs.Auth;

public class RefreshTokenDTO
{
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
}
namespace IntegrationTests.Common;

public static class JwtTokenProvider
{
    public static string Issuer { get; } = "identity-microservice-testing";
    public static string Audience { get; } = "identity-microservice-testing";

    public static SecurityKey SecurityKey { get; } = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("SecuenciaDeCaracteresSecretosParaTesting"));
    public static SigningCredentials SigningCredentials { get; } = new SigningCredentials(
        SecurityKey, SecurityAlgorithms.HmacSha256);
    internal static readonly JwtSecurityTokenHandler JwtSecurityTokenHandler = new();

    public static string GenerateSuperAdminAccessToken()
    {
        var superAdminToken = TokenWriter(
            "b6d099b2-2878-4bab-b40e-cbbdc73e2cd6", "SuperAdmin");

        return superAdminToken;
    }

    public static string GenerateModeratorAccessToken()
    {
        var moderatorToken = TokenWriter(
            "5065a1b2-f665-4a56-a3b8-3bac99e6db43", "Moderator");

        return moderatorToken;
    }

    private static string TokenWriter(string userId, string role)
    {
        var accesToken = JwtSecurityTokenHandler.WriteToken(
           new JwtSecurityToken(
               Issuer,
               Audience,
               new List<Claim> {
                new(ClaimTypes.NameIdentifier, userId),
                new(ClaimTypes.Role, role) },
               expires: DateTime.Now.AddMinutes(10),
               signingCredentials: SigningCredentials
           ));
        return accesToken;
    }
}
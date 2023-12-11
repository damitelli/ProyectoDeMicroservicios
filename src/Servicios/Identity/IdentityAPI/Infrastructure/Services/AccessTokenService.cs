namespace Infrastructure.Services;

internal sealed class AccessTokenService : IAccessTokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public AccessTokenService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    /// <summary>
    /// Genera un accessToken para la petición del usuario.
    /// </summary>
    /// <param name="appUser">Los datos del usuario sobre el que se debe
    /// generar el accessToken.</param>
    /// <returns>Retorna un accessToken.</returns>
    public async Task<string> CreateAccessTokenAsync(ApplicationUser appUser)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims(appUser);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var jwtConfig = _configuration.GetSection("jwtConfig");
        var key = Encoding.UTF8.GetBytes(jwtConfig["Secret"]);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims(ApplicationUser appUser)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, appUser.UserName),
            new(ClaimTypes.NameIdentifier, appUser.Id)
        };
        var roles = await _userManager.GetRolesAsync(appUser);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("JwtConfig");
        var tokenOptions = new JwtSecurityToken(
        issuer: jwtSettings["validIssuer"],
        audience: jwtSettings["validAudience"],
        claims: claims,
        expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expiresIn"])),
        signingCredentials: signingCredentials);
        return tokenOptions;
    }
}
namespace Infrastructure.Services;

internal sealed class RefreshTokenService : IRefreshTokenService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public RefreshTokenService(UserManager<ApplicationUser> userManager) => _userManager = userManager;

    /// <summary>
    /// Genera un refreshToken único en la base de datos para la petición del usuario.
    /// </summary>
    /// <returns>Retorna un refreshToken.</returns>
    public RefreshToken CreateRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = getUniqueToken(),
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow,
        };

        return refreshToken;

        string getUniqueToken()
        {
            // El token es una secuencia al azar de caracteres criptográficamente fuerte.
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            // Asegura que el token sea único contrastándolo con la base de datos.
            var tokenIsUnique = !_userManager.Users.Any(u => u.RefreshTokens.Any(t => t.Token == token));

            if (!tokenIsUnique)
                return getUniqueToken();

            return token;
        }
    }
}
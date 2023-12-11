namespace Infrastructure.Services;

internal sealed class UserAuthenticationService : IUserAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly IAccessTokenService _accessTokenService;
    private readonly IRefreshTokenService _refreshTokenService;

    public UserAuthenticationService(
        UserManager<ApplicationUser> userManager,
        IMapper mapper,
        IConfiguration configuration,
        IAccessTokenService accessTokenService,
        IRefreshTokenService refreshTokenService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _configuration = configuration;
        _accessTokenService = accessTokenService;
        _refreshTokenService = refreshTokenService;
    }

    /// <summary>
    /// Registra un nuevo usuario basado en los datos de la petición.
    /// </summary>
    /// <param name="request">Los datos del usuario enviados en la petición.</param>
    public async Task<IdentityResult> RegisterUserAsync(RegisterUserCommand request)
    {
        var user = _mapper.Map<ApplicationUser>(request.registerUserRequest);
        return await _userManager.CreateAsync(user, request.registerUserRequest.Password);
    }

    /// <summary>
    /// Autentica usuarios.
    /// </summary>
    /// <param name="request">Los datos del usuario.</param>
    /// <returns>Retorna un access token y un refresh token.</returns>
    public async Task<AuthenticateResponse> Authenticate(LoginUserCommand request)
    {
        var user = await _userManager.FindByNameAsync(request.loginUserRequest.UserName);

        // Valida el password proveniente de la petición.
        if (user == null || !await _userManager.CheckPasswordAsync(
            user, request.loginUserRequest.Password))
            throw new BadRequestException("Usuario o Contraseña incorrectos.");

        var accessToken = await _accessTokenService.CreateAccessTokenAsync(user);
        var refreshToken = _refreshTokenService.CreateRefreshToken();

        user.RefreshTokens.Add(refreshToken);

        await _userManager.UpdateAsync(user);

        return new AuthenticateResponse(accessToken, refreshToken.Token);
    }

    /// <summary>
    /// Genera nuevos tokens. (access token y refresh token).
    /// </summary>
    /// <param name="request">El refresh token a validar.</param>
    /// <returns>Retorna nuevos access token y un refresh token.</returns>
    public async Task<AuthenticateResponse> GenerateNewTokens(NewTokenCommand request)
    {
        var user = GetUserByRefreshToken(request.token);

        var refreshToken = user.RefreshTokens.Single(x => x.Token == request.token);

        if (refreshToken.IsRevoked)
        {
            // En caso de que el token enviado en la petición este comprometido,
            // revoca todos los tokens que desciendan del mismo.
            RevokeDescendantRefreshTokens(
                refreshToken,
                user,
                 $"Se intentó usar token ancestro revocado: {request.token}");

            await _userManager.UpdateAsync(user);
        }


        if (!refreshToken.IsActive)
            throw new InvalidRefreshTokenException();

        // Reemplaza el refresh token antiguo con uno nuevo.
        var newRefreshToken = RotateRefreshToken(refreshToken);
        user.RefreshTokens.Add(newRefreshToken);

        // Remueve tokens inactivos.
        RemoveOldRefreshTokens(user);

        await _userManager.UpdateAsync(user);

        var jwtToken = await _accessTokenService.CreateAccessTokenAsync(user);

        return new AuthenticateResponse(jwtToken, newRefreshToken.Token);
    }

    /// <summary>
    /// Revoca refresh tokens.
    /// </summary>
    /// <param name="request">El refresh token a revocar.</param>
    public async Task<Unit> RevokeRefreshToken(RebokeTokenCommand request)
    {
        if (request.token == null)
            throw new BadRequestException("Token requerido.");

        var user = GetUserByRefreshToken(request.token);
        var refreshToken = user.RefreshTokens.Single(x => x.Token == request.token);

        if (!refreshToken.IsActive)
            throw new InvalidRefreshTokenException();

        // Revoca tokens inhabilitándolos.
        RevokeRefreshToken(refreshToken, "Revocado sin reemplazo.");

        await _userManager.UpdateAsync(user);
        return Unit.Value;
    }



    // helper methods

    private ApplicationUser GetUserByRefreshToken(string token)
    {
        return _userManager.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token)) ?? throw new InvalidRefreshTokenException();
    }

    private void RevokeRefreshToken(RefreshToken token, string? reason = null, string? replacedByToken = null)
    {
        token.Revoked = DateTime.UtcNow;
        token.ReasonRevoked = reason;
        token.ReplacedByToken = replacedByToken;
    }

    private RefreshToken RotateRefreshToken(RefreshToken refreshToken)
    {
        var newRefreshToken = _refreshTokenService.CreateRefreshToken();
        RevokeRefreshToken(refreshToken, "Reemplazado por un nuevo token.", newRefreshToken.Token);
        return newRefreshToken;
    }

    private void RemoveOldRefreshTokens(ApplicationUser user)
    {
        // Remueve tokens inactivos del usuario basado en la propiedad TTL definida en app settings.
        var jwtConfig = _configuration.GetSection("jwtConfig");
        var refreshTokenTTL = _configuration.GetSection("RefreshTokenTTL");
        user.RefreshTokens.RemoveAll(x =>
            !x.IsActive &&
            x.Created.AddDays(Convert.ToDouble(refreshTokenTTL.Value)) <= DateTime.UtcNow);
    }

    private void RevokeDescendantRefreshTokens(RefreshToken refreshToken, ApplicationUser user, string reason)
    {
        // Se asegura que todos los tokens descendientes sean revocados.
        if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
        {
            var childToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);

            if (childToken.IsActive)
                RevokeRefreshToken(childToken, reason);

            RevokeDescendantRefreshTokens(childToken, user, reason);
        }
    }
}
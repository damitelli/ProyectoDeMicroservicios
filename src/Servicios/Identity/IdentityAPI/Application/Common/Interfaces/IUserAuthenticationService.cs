namespace Application.Common.Interfaces;

public interface IUserAuthenticationService
{
    Task<IdentityResult> RegisterUserAsync(RegisterUserCommand request);
    Task<AuthenticateResponse> Authenticate(LoginUserCommand loginDto);
    Task<AuthenticateResponse> GenerateNewTokens(NewTokenCommand request);
    Task<Unit> RevokeRefreshToken(RebokeTokenCommand request);
}
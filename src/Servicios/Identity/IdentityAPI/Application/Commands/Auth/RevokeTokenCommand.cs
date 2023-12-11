namespace Application.Commands.Auth;

public record RebokeTokenCommand(string token) : IRequestWrapper<Unit>;

internal sealed class RebokeTokenCommandHandler : IHandlerWrapper<RebokeTokenCommand, Unit>
{
    protected readonly IUserAuthenticationService _userAuthService;

    public RebokeTokenCommandHandler(IUserAuthenticationService userAuthService) =>
    _userAuthService = userAuthService;

    public async Task<IApiResponse<Unit>> Handle(
        RebokeTokenCommand request,
         CancellationToken cancellationToken) =>
            new ApiResponse<Unit>(await _userAuthService.RevokeRefreshToken(request));
}
namespace Application.Commands.Auth;

public record NewTokenCommand(string token) : IRequestWrapper<AuthenticateResponse>;

internal sealed class NewTokenCommandHandler : IHandlerWrapper<NewTokenCommand, AuthenticateResponse>
{
    protected readonly IUserAuthenticationService _userAuthService;

    public NewTokenCommandHandler(IUserAuthenticationService userAuthService) =>
    _userAuthService = userAuthService;

    public async Task<IApiResponse<AuthenticateResponse>> Handle(
        NewTokenCommand request,
        CancellationToken cancellationToken) =>
            new ApiResponse<AuthenticateResponse>(await _userAuthService.GenerateNewTokens(request));
}
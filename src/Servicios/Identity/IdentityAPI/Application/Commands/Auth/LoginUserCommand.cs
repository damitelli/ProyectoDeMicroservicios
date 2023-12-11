namespace Application.Commands.Auth;

public record LoginUserCommand(LoginUserRequest loginUserRequest) : IRequestWrapper<AuthenticateResponse>;

internal sealed class LoginUserCommandHandler : IHandlerWrapper<LoginUserCommand, AuthenticateResponse>
{
    protected readonly IUserAuthenticationService _userAuthService;

    public LoginUserCommandHandler(IUserAuthenticationService userAuthService) =>
     _userAuthService = userAuthService;

    public async Task<IApiResponse<AuthenticateResponse>> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken) =>
            new ApiResponse<AuthenticateResponse>(await _userAuthService.Authenticate(request));
}
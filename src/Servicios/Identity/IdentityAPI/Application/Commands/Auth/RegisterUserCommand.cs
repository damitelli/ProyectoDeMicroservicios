namespace Application.Commands.Auth;

public record RegisterUserCommand(RegisterUserRequest registerUserRequest) : IRequestWrapper<IdentityResult>;

internal sealed class RegisterUserCommandHandler : IHandlerWrapper<RegisterUserCommand, IdentityResult>
{
    protected readonly IUserAuthenticationService _userAuthService;

    public RegisterUserCommandHandler(IUserAuthenticationService userAuthService) =>
    _userAuthService = userAuthService;

    public async Task<IApiResponse<IdentityResult>> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken) =>
            new ApiResponse<IdentityResult>(await _userAuthService.RegisterUserAsync(request));
}
namespace Application.Commands.Users;

public record UpdateUserInfoCommand(string userId, UpdateUserInfoRequest updatedInfo) : IRequestWrapper<IdentityResult>;

internal sealed class UpdateUserInformationCommandHandler : IHandlerWrapper<UpdateUserInfoCommand, IdentityResult>
{
    protected readonly IUserService _userService;

    public UpdateUserInformationCommandHandler(IUserService userService) => _userService = userService;


    public async Task<IApiResponse<IdentityResult>> Handle(UpdateUserInfoCommand request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByIdAsync(request.userId) ??
         throw new UserNotFoundException(request.userId);

        if (user.UserName != request.updatedInfo.UserName)
            user.UserName = request.updatedInfo.UserName;

        if (user.FirstName != request.updatedInfo.FirstName)
            user.FirstName = request.updatedInfo.FirstName;

        if (user.LastName != request.updatedInfo.LastName)
            user.LastName = request.updatedInfo.LastName;

        if (user.Email != request.updatedInfo.Email)
            user.Email = request.updatedInfo.Email;

        return new ApiResponse<IdentityResult>(await _userService.UpdateUserAsync(user));
    }
}
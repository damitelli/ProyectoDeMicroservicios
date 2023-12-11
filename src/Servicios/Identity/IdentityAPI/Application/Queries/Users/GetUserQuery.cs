namespace Application.Queries.Users;

public record GetUserQuery(string userId) : IRequestWrapper<UserDTO>;

internal sealed class GetUserQueryHandler : IHandlerWrapper<GetUserQuery, UserDTO>
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<IApiResponse<UserDTO>> Handle(
        GetUserQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByIdAsync(request.userId) ??
        throw new UserNotFoundException(request.userId);

        return new ApiResponse<UserDTO>(_mapper.Map<UserDTO>(user));
    }
}
namespace Application.Queries.Users;

public record GetAllUsersQuery : IRequestWrapper<IReadOnlyList<UserDTO>>;

internal sealed class GetAllUserQueryHandler : IHandlerWrapper<GetAllUsersQuery, IReadOnlyList<UserDTO>>
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public GetAllUserQueryHandler(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<IApiResponse<IReadOnlyList<UserDTO>>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken)
    {
        var users = await _userService.GetAllUsersAsync();
        return new ApiResponse<IReadOnlyList<UserDTO>>(
            _mapper.Map<IReadOnlyList<UserDTO>>(users));
    }
}
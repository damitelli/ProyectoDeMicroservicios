namespace Application.Queries.Users;

public record GetRefreshTokenByUserIdQuery(string userId) : IRequestWrapper<IReadOnlyList<RefreshTokenDTO>>;

internal sealed class GetRefreshTokenByUserIdQueryHandler :
IHandlerWrapper<GetRefreshTokenByUserIdQuery, IReadOnlyList<RefreshTokenDTO>>
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public GetRefreshTokenByUserIdQueryHandler(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<IApiResponse<IReadOnlyList<RefreshTokenDTO>>> Handle(
        GetRefreshTokenByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByIdAsync(request.userId) ??
        throw new UserNotFoundException(request.userId);

        return new ApiResponse<IReadOnlyList<RefreshTokenDTO>>(
            _mapper.Map<IReadOnlyList<RefreshTokenDTO>>(user.RefreshTokens));
    }
}
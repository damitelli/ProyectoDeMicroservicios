namespace Application.Commands.UserFavoritelist;

public record GetUserFavoriteListByUserIdQuery(Guid userId) : IRequestWrapper<UserFavoriteListDTO>;

internal sealed class GetUserFavoriteListByUserIdQueryHandler : IHandlerWrapper<GetUserFavoriteListByUserIdQuery, UserFavoriteListDTO>
{
    private readonly IUserFavoriteListService _userFavoriteListService;

    private readonly IMapper _mapper;

    public GetUserFavoriteListByUserIdQueryHandler(
        IUserFavoriteListService userFavoriteListService,
        IMapper mapper)
    {
        _userFavoriteListService = userFavoriteListService;
        _mapper = mapper;
    }

    public async Task<IApiResponse<UserFavoriteListDTO>> Handle(
        GetUserFavoriteListByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var userFavoriteList = await _userFavoriteListService.GetUserFavoriteListByUserIdAsync(
            request.userId) ?? throw new UserFavoriteListNotFoundException(request.userId);

        return new ApiResponse<UserFavoriteListDTO>(_mapper.Map<UserFavoriteListDTO>(userFavoriteList));
    }
}

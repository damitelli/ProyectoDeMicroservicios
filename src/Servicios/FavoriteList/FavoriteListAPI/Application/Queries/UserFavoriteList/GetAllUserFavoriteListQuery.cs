namespace Application.Queries.UserFavoritelist;

public record GetAllUserFavoriteListQuery : IRequestWrapper<IReadOnlyCollection<UserFavoriteListDTO>>;

internal sealed class GetAllUserFavoriteListQueryHandler :
IHandlerWrapper<GetAllUserFavoriteListQuery, IReadOnlyCollection<UserFavoriteListDTO>>
{
    private readonly IUserFavoriteListService _userFavoriteListService;

    private readonly IMapper _mapper;

    public GetAllUserFavoriteListQueryHandler(
        IUserFavoriteListService userFavoriteListService,
        IMapper mapper)
    {
        _userFavoriteListService = userFavoriteListService;
        _mapper = mapper;
    }

    public async Task<IApiResponse<IReadOnlyCollection<UserFavoriteListDTO>>> Handle(
        GetAllUserFavoriteListQuery request,
        CancellationToken cancellationToken)
    {
        var allUserFavoriteList = await _userFavoriteListService.GetAllUserFavoriteListAsync();

        return new ApiResponse<IReadOnlyCollection<UserFavoriteListDTO>>(
            _mapper.Map<IReadOnlyCollection<UserFavoriteListDTO>>(allUserFavoriteList));
    }
}

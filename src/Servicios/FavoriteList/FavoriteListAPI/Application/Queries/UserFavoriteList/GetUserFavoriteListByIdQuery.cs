namespace Application.Commands.UserFavoritelist;

public record GetUserFavoriteListByIdQuery(Guid Id) : IRequestWrapper<UserFavoriteListDTO>;

internal sealed class GetUserFavoriteListByIdQueryHandler : IHandlerWrapper<GetUserFavoriteListByIdQuery, UserFavoriteListDTO>
{
    private readonly IUserFavoriteListService _userFavoriteListService;

    private readonly IMapper _mapper;

    public GetUserFavoriteListByIdQueryHandler(
        IUserFavoriteListService userFavoriteListService,
        IMapper mapper)
    {
        _userFavoriteListService = userFavoriteListService;
        _mapper = mapper;
    }

    public async Task<IApiResponse<UserFavoriteListDTO>> Handle(
        GetUserFavoriteListByIdQuery request,
        CancellationToken cancellationToken)
    {
        var userFavoriteList = await _userFavoriteListService.GetUserFavoriteListByIdAsync(
            request.Id) ??
             throw new UserFavoriteListNotFoundException(request.Id);

        return new ApiResponse<UserFavoriteListDTO>(_mapper.Map<UserFavoriteListDTO>(userFavoriteList));
    }
}

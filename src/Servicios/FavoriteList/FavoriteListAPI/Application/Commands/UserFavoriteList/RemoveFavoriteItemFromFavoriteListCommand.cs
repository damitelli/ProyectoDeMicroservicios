namespace Application.Commands.UserFavoritelist;

public record RemoveFavoriteItemFromFavoriteListCommand(Guid userId, Guid itemId) : IRequestWrapper<UserFavoriteListDTO>;

internal sealed class RemoveFavoriteItemFromFavoriteListCommandHandler : IHandlerWrapper<RemoveFavoriteItemFromFavoriteListCommand, UserFavoriteListDTO>
{
    private readonly IUserFavoriteListService _userFavoriteListService;

    private readonly IFavoriteItemService _favoriteItemService;

    private readonly IMapper _mapper;

    public RemoveFavoriteItemFromFavoriteListCommandHandler(
        IUserFavoriteListService userFavoriteListService,
        IFavoriteItemService favoriteItemService,
        IMapper mapper)
    {
        _userFavoriteListService = userFavoriteListService;
        _favoriteItemService = favoriteItemService;
        _mapper = mapper;
    }

    public async Task<IApiResponse<UserFavoriteListDTO>> Handle(
        RemoveFavoriteItemFromFavoriteListCommand request,
        CancellationToken cancellationToken)
    {
        var userFavoritelist = await _userFavoriteListService.
        GetUserFavoriteListByUserIdAsync(request.userId) ??
         throw new ListByUserIdNotFoundException(request.userId);

        var favoriteItem = await _favoriteItemService.
        GetFavoriteItemByIdAsync(request.itemId) ??
         throw new FavoriteItemNotFoundException(request.itemId);

        var favoriteItemToRemove = userFavoritelist.Items.
        FirstOrDefault(fitr => fitr.Id == request.itemId) ??
         throw new FavoriteItemNotFoundException(request.itemId);

        userFavoritelist.Items.Remove(favoriteItemToRemove);

        await _userFavoriteListService.UpdateUserFavoriteListAsync(userFavoritelist);

        return new ApiResponse<UserFavoriteListDTO>(_mapper.Map<UserFavoriteListDTO>(userFavoritelist));
    }
}
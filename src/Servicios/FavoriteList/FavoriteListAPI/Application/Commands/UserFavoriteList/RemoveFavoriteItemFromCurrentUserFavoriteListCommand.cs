namespace Application.Commands.UserFavoritelist;

public record RemoveFavoriteItemFromCurrentUserFavoriteListCommand(Guid itemId) : IRequestWrapper<UserFavoriteListDTO>;

internal sealed class RemoveFavoriteItemFromCurrentUserFavoriteListCommandHandler : IHandlerWrapper<RemoveFavoriteItemFromCurrentUserFavoriteListCommand, UserFavoriteListDTO>
{
    private readonly IUserFavoriteListService _userFavoriteListService;

    private readonly IFavoriteItemService _favoriteItemService;

    private readonly IIdentityService _identityService;

    private readonly IMapper _mapper;

    public RemoveFavoriteItemFromCurrentUserFavoriteListCommandHandler(
        IUserFavoriteListService userFavoriteListService,
        IFavoriteItemService favoriteItemService,
        IIdentityService identityService,
        IMapper mapper)
    {
        _userFavoriteListService = userFavoriteListService;
        _favoriteItemService = favoriteItemService;
        _identityService = identityService;
        _mapper = mapper;
    }

    public async Task<IApiResponse<UserFavoriteListDTO>> Handle(
        RemoveFavoriteItemFromCurrentUserFavoriteListCommand request,
        CancellationToken cancellationToken)
    {
        var userIdString = _identityService.GetUserId() ??
         throw new BadRequestException("No se encuentra el Token.");

        var userId = Guid.Parse(userIdString);

        var userFavoritelist = await _userFavoriteListService.
        GetUserFavoriteListByUserIdAsync(userId) ??
         throw new UserFavoriteListNotFoundException(userId);

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
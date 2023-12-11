namespace Application.Commands.UserFavoritelist;

public record AddFavoriteItemToUserFavoriteListCommand(
    Guid Id, Guid itemId) : IRequestWrapper<UserFavoriteListDTO>;

internal sealed class AddFavoriteItemToUserFavoriteListCommandCommandHandler : IHandlerWrapper<AddFavoriteItemToUserFavoriteListCommand, UserFavoriteListDTO>
{
    private readonly IUserFavoriteListService _userFavoriteListService;

    private readonly IFavoriteItemService _favoriteItemService;

    private readonly IMapper _mapper;

    public AddFavoriteItemToUserFavoriteListCommandCommandHandler(
        IUserFavoriteListService userFavoriteListService,
        IFavoriteItemService favoriteItemService,
        IMapper mapper)
    {
        _userFavoriteListService = userFavoriteListService;
        _favoriteItemService = favoriteItemService;
        _mapper = mapper;
    }

    public async Task<IApiResponse<UserFavoriteListDTO>> Handle(
        AddFavoriteItemToUserFavoriteListCommand request,
        CancellationToken cancellationToken)
    {
        var userFavoriteList = await _userFavoriteListService.
        GetUserFavoriteListByIdAsync(request.Id) ??
         throw new UserFavoriteListNotFoundException(request.Id);

        var favoriteItem = await _favoriteItemService.
        GetFavoriteItemByIdAsync(request.itemId) ??
         throw new FavoriteItemNotFoundException(request.itemId);

        if (userFavoriteList.Items.Any(fit => fit.Id == favoriteItem.Id))
            throw new InvalidOperationException("El item ya se encuentra en la lista de favoritos.");

        userFavoriteList.Items.Add(favoriteItem);

        await _userFavoriteListService.UpdateUserFavoriteListAsync(userFavoriteList);

        return new ApiResponse<UserFavoriteListDTO>(_mapper.Map<UserFavoriteListDTO>(userFavoriteList));
    }
}

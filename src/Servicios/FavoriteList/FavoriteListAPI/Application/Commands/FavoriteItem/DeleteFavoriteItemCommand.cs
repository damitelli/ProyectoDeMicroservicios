namespace Application.Commands.Favoriteitem;

public record DeleteFavoriteItemCommand(Guid ItemId) : IRequestWrapper<Unit>;

internal sealed class DeleteFavoriteItemCommandHandler : IHandlerWrapper<DeleteFavoriteItemCommand, Unit>
{
    private readonly IFavoriteItemService _favoriteItemService;

    private readonly IUserFavoriteListService _userFavoriteListService;

    public DeleteFavoriteItemCommandHandler(
        IFavoriteItemService favoriteItemService,
        IUserFavoriteListService userFavoriteListService)
    {
        _favoriteItemService = favoriteItemService;
        _userFavoriteListService = userFavoriteListService;
    }

    public async Task<IApiResponse<Unit>> Handle(
        DeleteFavoriteItemCommand request,
        CancellationToken cancellationToken)
    {
        var favoriteItem = await _favoriteItemService.GetFavoriteItemByIdAsync(request.ItemId) ??
            throw new FavoriteItemNotFoundException(request.ItemId);

        var allUserFavoriteList = await _userFavoriteListService.GetAllUserFavoriteListAsync();

        foreach (var ufavlist in allUserFavoriteList)
        {
            if (ufavlist.Items.Any(itr => itr.Id == favoriteItem.Id))
            {
                var favItemToRemove = ufavlist.Items.FirstOrDefault(itr => itr.Id == favoriteItem.Id) ?? throw new FavoriteItemNotFoundException(favoriteItem.Id);

                ufavlist.Items.Remove(favItemToRemove);
                await _userFavoriteListService.UpdateUserFavoriteListAsync(ufavlist);
            }
        }
        return new ApiResponse<Unit>(await _favoriteItemService.DeleteFavoriteItemAsync(favoriteItem.Id));
    }
}

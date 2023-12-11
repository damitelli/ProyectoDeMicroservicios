namespace Application.Consumers;

public class ItemRemovedFromFavoriteItemsConsumer : IConsumer<ItemRemovedFromFavoriteItems>
{
    private readonly IUserFavoriteListService _userFavoriteListService;

    private readonly IFavoriteItemService _favoriteItemService;

    public ItemRemovedFromFavoriteItemsConsumer(
        IUserFavoriteListService userFavoriteListService,
        IFavoriteItemService favoriteItemService)
    {
        _userFavoriteListService = userFavoriteListService;
        _favoriteItemService = favoriteItemService;
    }

    public async Task Consume(ConsumeContext<ItemRemovedFromFavoriteItems> context)
    {
        var message = context.Message ?? throw new ConnectionException(
            "No se puede conectar a message broker");

        var userFavoriteList = await _userFavoriteListService.GetUserFavoriteListByUserIdAsync(
            message.UserId) ?? throw new UserFavoriteListNotFoundException(message.UserId);

        var favoriteItem = await _favoriteItemService.GetFavoriteItemByIdAsync(message.ItemId) ??
         throw new FavoriteItemNotFoundException(message.ItemId);

        var favoriteItemToRemove = userFavoriteList.Items.FirstOrDefault(
            fitr => fitr.Id == favoriteItem.Id) ??
             throw new FavoriteItemNotFoundException(favoriteItem.Id);

        userFavoriteList.Items.Remove(favoriteItemToRemove);

        await _favoriteItemService.DeleteFavoriteItemAsync(favoriteItem.Id);

        await _userFavoriteListService.UpdateUserFavoriteListAsync(userFavoriteList);
    }
}
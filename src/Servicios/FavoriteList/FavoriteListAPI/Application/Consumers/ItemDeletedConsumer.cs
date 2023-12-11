namespace Application.Consumers;

public class ItemDeletedConsumer : IConsumer<ItemDeleted>
{
    private readonly IFavoriteItemService _favoriteItemService;

    private readonly IUserFavoriteListService _userFavoriteListService;

    public ItemDeletedConsumer(
        IFavoriteItemService favoriteItemService,
        IUserFavoriteListService userFavoriteListService)
    {
        _favoriteItemService = favoriteItemService;
        _userFavoriteListService = userFavoriteListService;
    }

    public async Task Consume(ConsumeContext<ItemDeleted> context)
    {
        var message = context.Message ?? throw new ConnectionException(
            "No se puede conectar a message broker");

        var favoriteItemWhitinListToDelete = await _favoriteItemService.GetFavoriteItemByIdAsync(
            message.ItemId) ?? throw new FavoriteItemNotFoundException(message.ItemId);

        var allUserFavoriteList = await _userFavoriteListService.GetAllUserFavoriteListAsync();

        foreach (var userFavoriteList in allUserFavoriteList)
        {
            if (userFavoriteList.Items.Any(fiwltd => fiwltd.Id == favoriteItemWhitinListToDelete.Id))
            {
                var favoriteItemToRemove = userFavoriteList.Items.FirstOrDefault(
                    fiwltd => fiwltd.Id == favoriteItemWhitinListToDelete.Id) ??
                     throw new FavoriteItemNotFoundException(favoriteItemWhitinListToDelete.Id);

                userFavoriteList.Items.Remove(favoriteItemToRemove);
                await _userFavoriteListService.UpdateUserFavoriteListAsync(userFavoriteList);
            }
        }
        await _favoriteItemService.DeleteFavoriteItemAsync(favoriteItemWhitinListToDelete.Id);
    }
}
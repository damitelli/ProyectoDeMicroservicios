namespace Application.Consumers;

public class ItemModifiedConsumer : IConsumer<ItemModified>
{
    private readonly IFavoriteItemService _favoriteItemService;

    private readonly IUserFavoriteListService _userFavoriteListService;

    public ItemModifiedConsumer(
        IFavoriteItemService favoriteItemService,
        IUserFavoriteListService userFavoriteListService)
    {
        _favoriteItemService = favoriteItemService;
        _userFavoriteListService = userFavoriteListService;
    }

    public async Task Consume(ConsumeContext<ItemModified> context)
    {
        var message = context.Message ?? throw new ConnectionException(
            "No se puede conectar a message broker");

        var favoriteItemToModify = await _favoriteItemService.GetFavoriteItemByIdAsync(
            message.ItemId) ?? throw new FavoriteItemNotFoundException(message.ItemId);

        favoriteItemToModify.Nombre = message.Nombre;
        favoriteItemToModify.Descripcion = message.Descripcion;
        favoriteItemToModify.Precio = message.Precio;

        await _favoriteItemService.UpdateFavoriteItemAsync(favoriteItemToModify);

        var userFavoriteLists = await _userFavoriteListService.GetAllUserFavoriteListAsync();

        foreach (var userFavoriteList in userFavoriteLists)
        {
            if (userFavoriteList.Items.Any(fim => fim.Id == favoriteItemToModify.Id))
            {
                var oldFavoriteItem =
                userFavoriteList.Items.FirstOrDefault(fim => fim.Id == favoriteItemToModify.Id) ??
                 throw new FavoriteItemNotFoundException(favoriteItemToModify.Id);

                userFavoriteList.Items.Remove(oldFavoriteItem);
                userFavoriteList.Items.Add(favoriteItemToModify);

                await _userFavoriteListService.UpdateUserFavoriteListAsync(userFavoriteList);
            }
        }
    }
}
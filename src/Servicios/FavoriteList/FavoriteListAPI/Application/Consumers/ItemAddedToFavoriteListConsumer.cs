namespace Application.Consumers;

public class ItemAddedToFavoriteListConsumer : IConsumer<ItemAddedToFavoriteList>
{
    private readonly IFavoriteItemService _favoriteItemService;

    private readonly IUserFavoriteListService _userFavoriteListService;
    public ItemAddedToFavoriteListConsumer(
        IFavoriteItemService favoriteItemService,
        IUserFavoriteListService userFavoriteListService)
    {
        _favoriteItemService = favoriteItemService;
        _userFavoriteListService = userFavoriteListService;
    }
    public async Task Consume(ConsumeContext<ItemAddedToFavoriteList> context)
    {
        var message = context.Message ?? throw new ConnectionException(
            "No se puede conectar a message broker");

        var userId = message.UserId;

        var userfavoritelist = await _userFavoriteListService.GetUserFavoriteListByUserIdAsync(
            Guid.Parse(userId));

        if (userfavoritelist == null)
        {
            userfavoritelist = new UserFavoriteList
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse(userId)
            };
            await _userFavoriteListService.CreateUserFavoriteListAsync(userfavoritelist);
        }

        var listItem = await _favoriteItemService.GetFavoriteItemByIdAsync(message.ItemId);

        if (listItem == null)
        {
            listItem = new FavoriteItem
            {
                Id = message.ItemId,
                Nombre = message.Nombre,
                Descripcion = message.Descripcion,
                Precio = message.Precio
            };
            await _favoriteItemService.CreateFavoriteItemAsync(listItem);
        }

        if (userfavoritelist.Items.Any(fit => fit.Id == listItem.Id))
            throw new InvalidOperationException("El item ya se encuentra en la lista de favoritos. Favorite List Consumer");

        userfavoritelist.Items.Add(listItem);
        await _userFavoriteListService.UpdateUserFavoriteListAsync(userfavoritelist);
    }
}
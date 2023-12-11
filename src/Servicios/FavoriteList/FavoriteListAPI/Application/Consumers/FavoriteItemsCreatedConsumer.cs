namespace Application.Consumers;

public class FavoriteItemsCreatedConsumer : IConsumer<FavoriteItemsCreated>
{
    private readonly IUserFavoriteListService _userFavoriteListService;

    public FavoriteItemsCreatedConsumer(IUserFavoriteListService userFavoriteListService) =>
     _userFavoriteListService = userFavoriteListService;
    public async Task Consume(ConsumeContext<FavoriteItemsCreated> context)
    {
        var message = context.Message ?? throw new ConnectionException(
            "No se puede conectar a message broker");

        var userFavoriteList = new UserFavoriteList
        {
            Id = Guid.NewGuid(),
            UserId = message.UserId
        };
        await _userFavoriteListService.CreateUserFavoriteListAsync(userFavoriteList);
    }
}

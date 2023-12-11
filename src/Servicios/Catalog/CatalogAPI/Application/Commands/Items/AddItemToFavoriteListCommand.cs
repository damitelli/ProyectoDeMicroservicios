namespace Application.Commands.Items;

public record AddItemToFavoriteListCommand(Guid itemId) : IRequestWrapper<string>;

internal sealed class AddItemToFavoriteListCommandHandler :
 IHandlerWrapper<AddItemToFavoriteListCommand, string>
{
    private readonly IItemService _itemService;

    private readonly IIdentityService _identityService;

    private readonly IPublishEndpoint _publishEndpoint;

    public AddItemToFavoriteListCommandHandler(
        IItemService itemService,
        IIdentityService identityService,
        IPublishEndpoint publishEndpoint)
    {
        _itemService = itemService;
        _identityService = identityService;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<IApiResponse<string>> Handle(
        AddItemToFavoriteListCommand request,
        CancellationToken cancellationToken)
    {
        var item = await _itemService.GetItemByIdAsync(request.itemId) ??
         throw new ItemNotFoundException(request.itemId);

        var userId = _identityService.GetUserId() ??
         throw new BadRequestException("No se encuentra el Token.");

        await _publishEndpoint.Publish(new ItemAddedToFavoriteList(
            userId, item.Id, item.Nombre, item.Descripcion, item.Precio));

        return new ApiResponse<string>(item.Nombre);
    }
}

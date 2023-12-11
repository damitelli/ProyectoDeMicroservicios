namespace Application.Commands.Items;

public record DeleteItemCommand(Guid itemId) : IRequestWrapper<Unit>;

internal sealed class DeleteItemCommandHandler : IHandlerWrapper<DeleteItemCommand, Unit>
{
    private readonly IItemService _itemService;

    private readonly IPublishEndpoint _publishEndpoint;

    public DeleteItemCommandHandler(
        IItemService itemService,
        IPublishEndpoint publishEndpoint)
    {
        _itemService = itemService;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<IApiResponse<Unit>> Handle(
        DeleteItemCommand request,
        CancellationToken cancellationToken)
    {
        var item = await _itemService.GetItemByIdAsync(request.itemId) ??
            throw new ItemNotFoundException(request.itemId);

        await _itemService.DeleteItemAsync(item.Id);

        await _publishEndpoint.Publish(new ItemDeleted(item.Id));

        return new ApiResponse<Unit>(Unit.Value);
    }
}

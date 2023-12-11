namespace Application.Commands.Items;

public record UpdateItemCommand(Guid itemId, UpdateItemRequest updatedInfo) : IRequestWrapper<ItemDTO>;

internal sealed class UpdateItemCommandHandler : IHandlerWrapper<UpdateItemCommand, ItemDTO>
{
    private readonly IItemService _itemService;

    private readonly IPublishEndpoint _publishEndpoint;

    private readonly IMapper _mapper;

    public UpdateItemCommandHandler(
    IItemService itemService,
    IPublishEndpoint publishEndpoint,
    IMapper mapper)
    {
        _itemService = itemService;
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
    }
    public async Task<IApiResponse<ItemDTO>> Handle(
        UpdateItemCommand request,
        CancellationToken cancellationToken)
    {
        var item = await _itemService.GetItemByIdAsync(request.itemId) ??
            throw new ItemNotFoundException(request.itemId);

        if (item.Nombre != request.updatedInfo.Nombre)
            item.Nombre = request.updatedInfo.Nombre;

        if (item.Descripcion != request.updatedInfo.Descripcion)
            item.Descripcion = request.updatedInfo.Descripcion;

        if (item.Precio != request.updatedInfo.Precio)
            item.Precio = request.updatedInfo.Precio;

        await _itemService.UpdateItemAsync(item);

        await _publishEndpoint.Publish(new ItemModified(item.Id, item.Nombre, item.Descripcion, item.Precio));

        return new ApiResponse<ItemDTO>(_mapper.Map<ItemDTO>(item));
    }
}

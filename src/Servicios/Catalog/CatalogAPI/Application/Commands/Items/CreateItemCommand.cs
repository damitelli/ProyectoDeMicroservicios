namespace Application.Commands.Items;

public record CreateItemCommand(CreateItemRequest creationInfo) : IRequestWrapper<ItemDTO>;

internal sealed class CreateItemCommandHandler : IHandlerWrapper<CreateItemCommand, ItemDTO>
{
    private readonly IItemService _itemService;

    private readonly IMapper _mapper;

    public CreateItemCommandHandler(IItemService itemService, IMapper mapper)
    {
        _itemService = itemService;
        _mapper = mapper;
    }

    public async Task<IApiResponse<ItemDTO>> Handle(
        CreateItemCommand request,
        CancellationToken cancellationToken)
    {
        var newItem = new Item
        {
            Id = Guid.NewGuid(),
            Nombre = request.creationInfo.Nombre,
            Descripcion = request.creationInfo.Descripcion,
            Precio = request.creationInfo.Precio,
            FechaDeCreacion = DateTimeOffset.UtcNow
        };
        await _itemService.CreateItemAsync(newItem);
        
        return new ApiResponse<ItemDTO>(_mapper.Map<ItemDTO>(newItem));
    }
}
namespace Application.Queries.item;

public record GetItemByIdQuery(Guid itemId) : IRequestWrapper<ItemDTO>;

internal sealed class GetItemByIdQueryHandler : IHandlerWrapper<GetItemByIdQuery, ItemDTO>
{
    private readonly IItemService _itemService;

    private readonly IMapper _mapper;

    public GetItemByIdQueryHandler(IItemService itemService, IMapper mapper)
    {
        _itemService = itemService;
        _mapper = mapper;
    }

    public async Task<IApiResponse<ItemDTO>> Handle(
        GetItemByIdQuery request,
        CancellationToken cancellationToken)
    {
        var item = await _itemService.GetItemByIdAsync(request.itemId) ??
            throw new ItemNotFoundException(request.itemId);

        return new ApiResponse<ItemDTO>(_mapper.Map<ItemDTO>(item));
    }
}

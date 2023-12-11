namespace Application.Queries.item;

public record GetAllItemsQuery : IRequestWrapper<IReadOnlyCollection<ItemDTO>>;

internal sealed class GetAllItemsQueryHandler :
IHandlerWrapper<GetAllItemsQuery, IReadOnlyCollection<ItemDTO>>
{
    private readonly IItemService _itemService;

    private readonly IMapper _mapper;

    public GetAllItemsQueryHandler(IItemService itemService, IMapper mapper)
    {
        _itemService = itemService;
        _mapper = mapper;
    }

    public async Task<IApiResponse<IReadOnlyCollection<ItemDTO>>> Handle(
        GetAllItemsQuery request,
        CancellationToken cancellationToken)
    {
        var items = await _itemService.GetAllItemsAsync();

        return new ApiResponse<IReadOnlyCollection<ItemDTO>>(_mapper.Map<IReadOnlyCollection<ItemDTO>>(items));
    }
}

namespace Application.Queries.Favoriteitems;

public record GetFavoriteItemByIdQuery(Guid itemId) : IRequestWrapper<FavoriteItemDTO>;

internal sealed class GetFavoriteItemByIdQueryHandler :
IHandlerWrapper<GetFavoriteItemByIdQuery, FavoriteItemDTO>
{
    private readonly IFavoriteItemService _favoriteItemService;

    private readonly IMapper _mapper;

    public GetFavoriteItemByIdQueryHandler(IFavoriteItemService favoriteItemService, IMapper mapper)
    {
        _favoriteItemService = favoriteItemService;
        _mapper = mapper;
    }

    public async Task<IApiResponse<FavoriteItemDTO>> Handle(
        GetFavoriteItemByIdQuery request,
        CancellationToken cancellationToken)
    {
        var item = await _favoriteItemService.GetFavoriteItemByIdAsync(request.itemId) ??
            throw new FavoriteItemNotFoundException(request.itemId);

        return new ApiResponse<FavoriteItemDTO>(_mapper.Map<FavoriteItemDTO>(item));
    }
}

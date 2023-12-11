namespace Application.Queries.Favoriteitems;

public record GetAllFavoriteItemQuery : IRequestWrapper<IReadOnlyCollection<FavoriteItemDTO>>;

internal sealed class GetAllFavoriteItemQueryHandler :
IHandlerWrapper<GetAllFavoriteItemQuery, IReadOnlyCollection<FavoriteItemDTO>>
{
    private readonly IFavoriteItemService _favoriteItemService;

    private readonly IMapper _mapper;

    public GetAllFavoriteItemQueryHandler(IFavoriteItemService favoriteItemService, IMapper mapper)
    {
        _favoriteItemService = favoriteItemService;
        _mapper = mapper;
    }

    public async Task<IApiResponse<IReadOnlyCollection<FavoriteItemDTO>>> Handle(
        GetAllFavoriteItemQuery request,
        CancellationToken cancellationToken)
    {
        var allFavoriteItem = await _favoriteItemService.GetAllFavoriteItemAsync();

        return new ApiResponse<IReadOnlyCollection<FavoriteItemDTO>>(
            _mapper.Map<IReadOnlyCollection<FavoriteItemDTO>>(allFavoriteItem));
    }
}

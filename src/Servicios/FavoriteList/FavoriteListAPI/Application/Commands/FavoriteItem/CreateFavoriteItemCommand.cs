namespace Application.Commands.Favoriteitem;

public record CreateFavoriteItemCommand(
    CreateFavoriteItemRequest creationInfo) : IRequestWrapper<FavoriteItemDTO>;

internal sealed class CreateFavoriteItemCommandHandler :
IHandlerWrapper<CreateFavoriteItemCommand, FavoriteItemDTO>
{
    private readonly IFavoriteItemService _favoriteItemService;

    private readonly IMapper _mapper;

    public CreateFavoriteItemCommandHandler(
        IFavoriteItemService favoriteItemService,
        IMapper mapper)
    {
        _favoriteItemService = favoriteItemService;
        _mapper = mapper;
    }

    public async Task<IApiResponse<FavoriteItemDTO>> Handle(
        CreateFavoriteItemCommand request,
        CancellationToken cancellationToken)
    {
        var newFavoriteItem = new FavoriteItem
        {
            Id = Guid.NewGuid(),
            Nombre = request.creationInfo.Nombre,
            Descripcion = request.creationInfo.Descripcion,
            Precio = request.creationInfo.Precio
        };
        await _favoriteItemService.CreateFavoriteItemAsync(newFavoriteItem);
        return new ApiResponse<FavoriteItemDTO>(_mapper.Map<FavoriteItemDTO>(newFavoriteItem));
    }
}
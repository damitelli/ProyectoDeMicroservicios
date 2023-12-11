namespace Application.Commands.Favoriteitem;

public record UpdateFavoriteItemCommand(
    Guid ItemId, UpdateFavoriteItemRequest updatedInfo) : IRequestWrapper<FavoriteItemDTO>;

internal sealed class UpdateFavoriteItemCommandHandler :
IHandlerWrapper<UpdateFavoriteItemCommand, FavoriteItemDTO>
{
    private readonly IFavoriteItemService _favoriteItemService;

    private readonly IUserFavoriteListService _userFavoriteListService;

    private readonly IMapper _mapper;

    public UpdateFavoriteItemCommandHandler(
        IFavoriteItemService favoriteItemService,
        IUserFavoriteListService userFavoriteListService,
        IMapper mapper)
    {
        _favoriteItemService = favoriteItemService;
        _userFavoriteListService = userFavoriteListService;
        _mapper = mapper;
    }
    public async Task<IApiResponse<FavoriteItemDTO>> Handle(
        UpdateFavoriteItemCommand request,
        CancellationToken cancellationToken)
    {
        var favoriteItem = await _favoriteItemService.GetFavoriteItemByIdAsync(request.ItemId) ??
        throw new FavoriteItemNotFoundException(request.ItemId);

        if (favoriteItem.Nombre != request.updatedInfo.Nombre)
            favoriteItem.Nombre = request.updatedInfo.Nombre;

        if (favoriteItem.Descripcion != request.updatedInfo.Descripcion)
            favoriteItem.Descripcion = request.updatedInfo.Descripcion;

        if (favoriteItem.Precio != request.updatedInfo.Precio)
            favoriteItem.Precio = request.updatedInfo.Precio;

        var allUserFavoriteList = await _userFavoriteListService.GetAllUserFavoriteListAsync();

        foreach (var ufavlist in allUserFavoriteList)
        {
            if (ufavlist.Items.Any(itc => itc.Id == favoriteItem.Id))
            {
                var favItemToRemove = ufavlist.Items.FirstOrDefault(itr => itr.Id == favoriteItem.Id) ?? throw new FavoriteItemNotFoundException(favoriteItem.Id);

                ufavlist.Items.Remove(favItemToRemove);
                ufavlist.Items.Add(favoriteItem);

                await _userFavoriteListService.UpdateUserFavoriteListAsync(ufavlist);
            }
        }
        await _favoriteItemService.UpdateFavoriteItemAsync(favoriteItem);

        return new ApiResponse<FavoriteItemDTO>(_mapper.Map<FavoriteItemDTO>(favoriteItem));
    }
}

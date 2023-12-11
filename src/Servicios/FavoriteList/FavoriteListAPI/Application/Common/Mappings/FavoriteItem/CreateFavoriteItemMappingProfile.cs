namespace Application.Common.Mappings.Favoriteitem;

public class CreateFavoriteItemMappingProfile : Profile
{
    public CreateFavoriteItemMappingProfile()
    {
        CreateMap<CreateFavoriteItemRequest, FavoriteItem>();
    }
}
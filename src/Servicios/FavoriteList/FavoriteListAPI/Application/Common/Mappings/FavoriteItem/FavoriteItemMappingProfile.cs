namespace Application.Common.Mappings.Favoriteitem;

public class FavoriteItemMappingProfile : Profile
{
    public FavoriteItemMappingProfile()
    {
        CreateMap<FavoriteItem, FavoriteItemDTO>();

        CreateMap<UpdateFavoriteItemRequest, FavoriteItem>().ReverseMap();
    }
}
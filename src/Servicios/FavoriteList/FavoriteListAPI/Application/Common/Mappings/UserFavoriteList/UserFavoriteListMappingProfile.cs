namespace Application.Common.Mappings.UserFavoritelist;

public class UserFavoriteListMappingProfile : Profile
{
    public UserFavoriteListMappingProfile()
    {
        CreateMap<UserFavoriteList, UserFavoriteListDTO>();
    }
}
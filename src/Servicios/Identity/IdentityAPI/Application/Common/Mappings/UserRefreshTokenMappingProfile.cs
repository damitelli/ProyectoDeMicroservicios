namespace Application.Common.Mappings;

public class UserRefreshTokenMappingProfile : Profile
{
    public UserRefreshTokenMappingProfile()
    {
        CreateMap<RefreshToken, RefreshTokenDTO>();
    }
}
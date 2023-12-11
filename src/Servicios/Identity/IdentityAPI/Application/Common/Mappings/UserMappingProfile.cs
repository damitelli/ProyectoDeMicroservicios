namespace Application.Common.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<ApplicationUser, UserDTO>();

        CreateMap<UpdateUserInfoRequest, ApplicationUser>().ReverseMap();
    }
}
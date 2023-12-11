namespace Application.Common.Mappings;

public class UserRegistrationMappingProfile : Profile
{
    public UserRegistrationMappingProfile()
    {
        CreateMap<RegisterUserRequest, ApplicationUser>();
    }
}

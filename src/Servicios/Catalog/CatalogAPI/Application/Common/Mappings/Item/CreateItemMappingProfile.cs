namespace Application.Common.Mappings.Items;

public class CreateItemMappingProfile : Profile
{
    public CreateItemMappingProfile()
    {
        CreateMap<CreateItemRequest, Item>();
    }
}
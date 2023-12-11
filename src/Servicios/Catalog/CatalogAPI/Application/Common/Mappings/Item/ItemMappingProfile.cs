namespace Application.Common.Mappings.Items;

public class ItemMappingProfile : Profile
{
    public ItemMappingProfile()
    {
        CreateMap<Item, ItemDTO>();

        CreateMap<UpdateItemRequest, Item>().ReverseMap();
    }
}
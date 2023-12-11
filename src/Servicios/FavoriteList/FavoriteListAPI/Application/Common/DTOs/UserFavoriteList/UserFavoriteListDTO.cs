namespace Application.Common.DTOs.UserFavoriteList;

public class UserFavoriteListDTO
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public List<FavoriteItemDTO> Items { get; set; }
}
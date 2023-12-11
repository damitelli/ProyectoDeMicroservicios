namespace Domain.Entities;

public class UserFavoriteList
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public IList<FavoriteItem> Items { get; set; } = new List<FavoriteItem>();
}
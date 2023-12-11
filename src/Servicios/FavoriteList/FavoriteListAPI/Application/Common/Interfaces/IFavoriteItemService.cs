namespace Application.Common.Interfaces;

public interface IFavoriteItemService
{
    Task<IReadOnlyCollection<FavoriteItem>> GetAllFavoriteItemAsync();
    Task<FavoriteItem> GetFavoriteItemByIdAsync(Guid id);
    Task CreateFavoriteItemAsync(FavoriteItem favoriteItem);
    Task UpdateFavoriteItemAsync(FavoriteItem favoriteItem);
    Task<Unit> DeleteFavoriteItemAsync(Guid id);
}
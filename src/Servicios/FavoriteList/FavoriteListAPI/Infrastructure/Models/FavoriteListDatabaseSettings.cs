namespace Infrastructure.Models;

public class FavoriteListDatabaseSettings : IFavoriteListDatabaseSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string FavoriteItemCollectionName { get; set; }
    public string UserFavoriteListCollectionName { get; set; }
}

public interface IFavoriteListDatabaseSettings
{
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
    string FavoriteItemCollectionName { get; set; }
    string UserFavoriteListCollectionName { get; set; }
}
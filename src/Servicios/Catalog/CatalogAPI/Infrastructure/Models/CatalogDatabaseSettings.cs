namespace Infrastructure.Models;

public class CatalogDatabaseSettings : ICatalogDatabaseSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string ItemCollectionName { get; set; }
}

public interface ICatalogDatabaseSettings
{
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
    string ItemCollectionName { get; set; }
}
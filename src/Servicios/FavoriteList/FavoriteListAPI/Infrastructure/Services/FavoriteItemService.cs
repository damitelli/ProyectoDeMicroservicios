namespace Infrastructure.Services;

internal sealed class FavoriteItemService : IFavoriteItemService
{
    private readonly IMongoCollection<FavoriteItem> _dbCollection;

    private readonly FilterDefinitionBuilder<FavoriteItem> _filterBuilder = Builders<FavoriteItem>.Filter;

    public FavoriteItemService(IFavoriteListDatabaseSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _dbCollection = database.GetCollection<FavoriteItem>(settings.FavoriteItemCollectionName);
    }

    public async Task<IReadOnlyCollection<FavoriteItem>> GetAllFavoriteItemAsync() =>
     await _dbCollection.Find(_filterBuilder.Empty).ToListAsync();

    public async Task<FavoriteItem> GetFavoriteItemByIdAsync(Guid id)
    {
        FilterDefinition<FavoriteItem> filter = _filterBuilder.Eq(favitem => favitem.Id, id);

        return await _dbCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task CreateFavoriteItemAsync(FavoriteItem favoriteItem)
    {
        await _dbCollection.InsertOneAsync(favoriteItem);
    }

    public async Task UpdateFavoriteItemAsync(FavoriteItem favoriteItem)
    {
        FilterDefinition<FavoriteItem> filter = _filterBuilder.Eq(fitup => fitup.Id, favoriteItem.Id);

        await _dbCollection.ReplaceOneAsync(filter, favoriteItem);
    }

    public async Task<Unit> DeleteFavoriteItemAsync(Guid id)
    {
        FilterDefinition<FavoriteItem> filter = _filterBuilder.Eq(fitdlt => fitdlt.Id, id);

        await _dbCollection.DeleteOneAsync(filter);

        return Unit.Value;
    }
}

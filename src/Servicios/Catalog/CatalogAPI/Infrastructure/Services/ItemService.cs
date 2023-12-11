namespace Infrastructure.Services;

internal sealed class ItemService : IItemService
{
    private readonly IMongoCollection<Item> _dbCollection;

    private readonly FilterDefinitionBuilder<Item> _filterBuilder = Builders<Item>.Filter;

    public ItemService(ICatalogDatabaseSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _dbCollection = database.GetCollection<Item>(settings.ItemCollectionName);
    }

    public async Task<IReadOnlyCollection<Item>> GetAllItemsAsync() =>
     await _dbCollection.Find(_filterBuilder.Empty).ToListAsync();


    public async Task<Item> GetItemByIdAsync(Guid id)
    {
        FilterDefinition<Item> filter = _filterBuilder.Eq(item => item.Id, id);

        return await _dbCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task CreateItemAsync(Item item) => await _dbCollection.InsertOneAsync(item);


    public async Task UpdateItemAsync(Item item)
    {
        FilterDefinition<Item> filter = _filterBuilder.Eq(itup => itup.Id, item.Id);

        await _dbCollection.ReplaceOneAsync(filter, item);
    }

    public async Task<Unit> DeleteItemAsync(Guid id)
    {
        FilterDefinition<Item> filter = _filterBuilder.Eq(itdlt => itdlt.Id, id);

        await _dbCollection.DeleteOneAsync(filter);

        return Unit.Value;
    }
}

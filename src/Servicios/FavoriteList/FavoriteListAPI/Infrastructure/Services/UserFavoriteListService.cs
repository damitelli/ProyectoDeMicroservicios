namespace Infrastructure.Services;

internal sealed class UserFavoriteListService : IUserFavoriteListService
{
    private readonly IMongoCollection<UserFavoriteList> _dbCollection;

    private readonly FilterDefinitionBuilder<UserFavoriteList> _filterBuilder = Builders<UserFavoriteList>.Filter;

    public UserFavoriteListService(IFavoriteListDatabaseSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _dbCollection = database.GetCollection<UserFavoriteList>(settings.UserFavoriteListCollectionName);
    }

    public async Task<IReadOnlyCollection<UserFavoriteList>> GetAllUserFavoriteListAsync() =>
     await _dbCollection.Find(_filterBuilder.Empty).ToListAsync();

    public async Task<UserFavoriteList> GetUserFavoriteListByIdAsync(Guid Id)
    {
        FilterDefinition<UserFavoriteList> filter = _filterBuilder.Eq(uflist => uflist.Id, Id);

        return await _dbCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<UserFavoriteList> GetUserFavoriteListByUserIdAsync(Guid userId)
    {
        FilterDefinition<UserFavoriteList> filter = _filterBuilder.Eq(uflist => uflist.UserId, userId);

        return await _dbCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task CreateUserFavoriteListAsync(UserFavoriteList userFavoriteList)
    {
        await _dbCollection.InsertOneAsync(userFavoriteList);
    }

    public async Task UpdateUserFavoriteListAsync(UserFavoriteList userFavoriteList)
    {
        FilterDefinition<UserFavoriteList> filter = _filterBuilder.Eq(uflistup => uflistup.Id, userFavoriteList.Id);

        await _dbCollection.FindOneAndReplaceAsync(filter, userFavoriteList);
    }
}

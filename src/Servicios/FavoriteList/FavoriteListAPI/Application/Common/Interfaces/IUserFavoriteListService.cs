namespace Application.Common.Interfaces;

public interface IUserFavoriteListService
{
    Task<IReadOnlyCollection<UserFavoriteList>> GetAllUserFavoriteListAsync();
    Task<UserFavoriteList> GetUserFavoriteListByIdAsync(Guid Id);
    Task<UserFavoriteList> GetUserFavoriteListByUserIdAsync(Guid userId);
    Task CreateUserFavoriteListAsync(UserFavoriteList userFavoriteList);
    Task UpdateUserFavoriteListAsync(UserFavoriteList userFavoriteList);
}
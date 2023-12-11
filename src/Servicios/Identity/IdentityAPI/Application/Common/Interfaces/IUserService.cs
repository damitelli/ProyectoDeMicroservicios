namespace Application.Common.Interfaces;

public interface IUserService
{
    Task<IReadOnlyList<ApplicationUser>> GetAllUsersAsync();
    Task<ApplicationUser> GetUserByIdAsync(string userId);
    Task<ApplicationUser> FindUserByNameAsync(string userName);
    Task<ApplicationUser> FindUserByEmailAsync(string userName);
    Task<IdentityResult> UpdateUserAsync(ApplicationUser user);
}
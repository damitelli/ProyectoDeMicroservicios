namespace Infrastructure.Services;

internal sealed class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager) => _userManager = userManager;

    public async Task<IReadOnlyList<ApplicationUser>> GetAllUsersAsync()
    {
        return await _userManager.Users.ToListAsync();
    }

    public async Task<ApplicationUser> GetUserByIdAsync(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }

    public async Task<ApplicationUser> FindUserByNameAsync(string userName)
    {
        return await _userManager.FindByNameAsync(userName);
    }

    public async Task<ApplicationUser> FindUserByEmailAsync(string userName)
    {
        return await _userManager.FindByEmailAsync(userName);
    }

    public async Task<IdentityResult> UpdateUserAsync(ApplicationUser user)
    {
        return await _userManager.UpdateAsync(user);
    }
}

using KERP.Domain.Identity;
using KERP.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;

namespace KERP.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApplicationUser?> GetByIdAsync(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }
}
using KERP.Core.Identity;
using KERP.Core.Interfaces.Repositories;
using KERP.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace KERP.Core.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;

    // Wstrzykujemy obie zależności
    public UserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
    }

    public string? GetCurrentUserId()
    {
        // Szybka operacja bez dostępu do bazy
        return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
    {
        // Operacja wymagająca dostępu do bazy poprzez repozytorium
        return await _userRepository.GetByIdAsync(userId);
    }
}

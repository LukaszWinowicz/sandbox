using KERP.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace KERP.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // Pobieramy ID użytkownika z jego "Claims"
    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    // Pobieramy ID fabryki z ciasteczka o nazwie "FactoryId"
    public int? SelectedFactoryId
    {
        get
        {
            var factoryIdCookie = _httpContextAccessor.HttpContext?.Request.Cookies["FactoryId"];
            if (int.TryParse(factoryIdCookie, out int factoryId))
            {
                return factoryId;
            }
            return null;
        }
    }
}
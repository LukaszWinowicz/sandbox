using KERP.Application.Common.Context;
using Microsoft.AspNetCore.Http;

namespace KERP.Infrastructure.Auth;

public class CurrentUserContext : ICurrentUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserId => "dev-user-001";
    public int FactoryId => 241;

    //public string UserId =>
    //    _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value ?? "anonymous";

    //public int FactoryId
    //{
    //    get
    //    {
    //        var cookieValue = _httpContextAccessor.HttpContext?.Request?.Cookies["factoryId"];
    //        return int.TryParse(cookieValue, out var result) ? result : 0;
    //    }
    //}
}
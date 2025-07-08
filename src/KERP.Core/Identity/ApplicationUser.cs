using Microsoft.AspNetCore.Identity;

namespace KERP.Core.Identity;

/// <summary>
/// Extends the default IdentityUser with application-specific properties.
/// </summary>
public class ApplicationUser : IdentityUser
{
    // W przyszłości dodamy tu np.:
    // public int? DefaultFactoryId { get; set; }
}

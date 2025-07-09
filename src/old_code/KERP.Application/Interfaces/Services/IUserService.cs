using KERP.Domain.Identity;

namespace KERP.Application.Interfaces.Services;

/// <summary>
/// Defines a contract for a service that provides information about the current user.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Gets the unique ID of the currently authenticated user.
    /// </summary>
    string? GetCurrentUserId();

    /// <summary>
    /// Gets the full user object from the database by its ID.
    /// </summary>
    Task<ApplicationUser?> GetUserByIdAsync(string userId);
}

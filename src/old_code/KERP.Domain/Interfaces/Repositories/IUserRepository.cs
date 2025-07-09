using KERP.Domain.Identity;

namespace KERP.Domain.Interfaces.Repositories;

/// <summary>
/// Defines a contract for a repository that handles user-related data operations.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Finds a user by their unique ID.
    /// </summary>
    /// <param name="userId">The ID of the user to find.</param>
    /// <returns>The ApplicationUser object or null if not found.</returns>
    Task<ApplicationUser?> GetByIdAsync(string userId);
}

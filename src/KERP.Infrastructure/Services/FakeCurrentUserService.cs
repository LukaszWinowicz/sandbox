using KERP.Application.Interfaces;

namespace KERP.Infrastructure.Services;

/// <summary>
/// Fałszywa implementacja ICurrentUserService używana tylko w środowisku deweloperskim,
/// aby ominąć potrzebę logowania podczas testów.
/// </summary>
public class FakeCurrentUserService : ICurrentUserService
{
    // Zawsze zwracamy stałe ID użytkownika
    public string? UserId => "DEV_USER"; // Możesz wpisać cokolwiek

    // Zawsze zwracamy stałe ID fabryki, aby uniknąć kolejnego błędu
    public int? SelectedFactoryId => 241; // Wybierz jedno z istniejących ID
}

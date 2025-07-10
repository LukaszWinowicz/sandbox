namespace KERP.Application.Interfaces;

/// <summary>
/// Zapewnia dostęp do informacji o bieżącym użytkowniku i jego kontekście.
/// </summary>
public interface ICurrentUserService
{
    string? UserId { get; }
    int? SelectedFactoryId { get; }
}

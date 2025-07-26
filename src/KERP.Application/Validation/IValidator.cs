using System.Windows.Input;

namespace KERP.Application.Validation;

/// <summary>
/// Definiuje walidatora dla komendy.
/// </summary>
/// <typeparam name="TComman">Typ komendy do walidacji.</typeparam>
public interface IValidator<in TComman> where TComman : ICommand
{
    ValidationResult Validate(TComman comman);
}
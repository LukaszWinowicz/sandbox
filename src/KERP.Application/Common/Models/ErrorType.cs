namespace KERP.Application.Common.Models;

/// <summary>
/// Definujemy typy błędów w systemie.
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// Błąd krytyczny, który uniemożliwia dalsze operacje.
    /// </summary>
    Critical,

    /// <summary>
    /// Ostrzeżenie, które nie blokuje operacji, ale inofrmuje o potencjalnych problemach.
    /// </summary>
    Warning
}

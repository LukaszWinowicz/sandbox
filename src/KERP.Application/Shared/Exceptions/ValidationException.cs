namespace KERP.Application.Shared.Exceptions;

public class ValidationException : Exception
{
    /// <summary>
    /// Kolekcja błędów walidacyjnych, które wystąpiły.
    /// </summary>
    public IReadOnlyCollection<string> Errors { get; }

    /// <summary>
    /// Konstruktor, który przyjmuje kolekcję błędów.
    /// </summary>
    /// <param name="errors">Kolekcja wiadomości o błędach.</param>
    public ValidationException(IReadOnlyCollection<string> errors)
        : base("One or more validation failures have occurred.") // Generyczna wiadomość główna
    {
        Errors = errors;
    }
}
﻿namespace KERP.Application.Validation;

/// <summary>
/// Reprezentuje rezultat walidacji wejściowej.
/// </summary>
public class ValidationResult(IReadOnlyCollection<ValidationError> Errors)
{
    public static ValidationResult Success() => new ValidationResult(Array.Empty<ValidationError>());
}

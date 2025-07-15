namespace KERP.Domain.Enums;

/// <summary>
/// Określa, jaki typ daty przyjęcia ma zostać zaktualizowany w zamówieniu zakupu.
/// </summary>
public enum ReceiptDateUpdateType
{
    /// <summary>
    /// Aktualizacja potwierdzonej daty przyjęcia (Confirmed Receipt Date).
    /// </summary>
    Confirmed = 1,

    /// <summary>
    /// Aktualizacja planowanej daty przyjęcia (Planned Receipt Date).
    /// </summary>
    Planned = 2
}

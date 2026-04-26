using Sondor.Translations;

namespace Sondor.ResultManager;

/// <summary>
/// The <see cref="Errors.SondorResult"/> manager.
/// </summary>
/// <param name="translationManager">The translation manager.</param>
public class SondorResultManager(ISondorTranslationManager translationManager) :
    ISondorResultManager
{
    /// <inheritdoc />
    public ISondorTranslationManager TranslationManager { get; } = translationManager;
}
using Microsoft.AspNetCore.Http;
using Sondor.Translations;

namespace Sondor.ResultManager;

/// <summary>
/// The <see cref="Errors.SondorResult"/> manager.
/// </summary>
public class SondorResultManager(IHttpContextAccessor contextAccessor,
    ISondorTranslationManager translationManager) :
    ISondorResultManager
{
    /// <summary>
    /// The HTTP context accessor.
    /// </summary>
    public IHttpContextAccessor HttpContextAccessor { get; } = contextAccessor;

    /// <inheritdoc />
    public ISondorTranslationManager TranslationManager { get; } = translationManager;
}
using Microsoft.AspNetCore.Http;
using Sondor.Errors;
using Sondor.Translations;

namespace Sondor.ResultManager;

/// <summary>
/// The marker interface, for <see cref="SondorResult"/> management.
/// </summary>
public interface ISondorResultManager
{
    /// <summary>
    /// The HTTP context accessor.
    /// </summary>
    IHttpContextAccessor HttpContextAccessor { get; }

    /// <summary>
    /// The translation manager.
    /// </summary>
    ISondorTranslationManager TranslationManager { get; }
}
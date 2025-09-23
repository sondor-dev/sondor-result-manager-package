using Microsoft.AspNetCore.Http;
using Sondor.Translations;

namespace Sondor.ResultManager.Tests;

/// <summary>
/// The test result manager.
/// </summary>
/// <remarks>
/// Create a new instance of <see cref="TestResultManager"/>.
/// </remarks>
/// <param name="translationManager">The translation manager.</param>
/// <param name="contextAccessor">The HTTP context accessor.</param>
internal class TestResultManager(ISondorTranslationManager translationManager,
    IHttpContextAccessor contextAccessor) :
    SondorResultManager(contextAccessor, translationManager);
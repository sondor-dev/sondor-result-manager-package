using Sondor.Translations;

namespace Sondor.ResultManager.Tests;

/// <summary>
/// The test result manager.
/// </summary>
/// <remarks>
/// Create a new instance of <see cref="TestResultManager"/>.
/// </remarks>
/// <param name="translationManager">The translation manager.</param>
internal class TestResultManager(ISondorTranslationManager translationManager) :
    SondorResultManager(translationManager);
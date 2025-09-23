using Microsoft.Extensions.DependencyInjection;
using Sondor.ResultManager.Extensions;
using Sondor.Translations.Args;
using Sondor.Translations.Extensions;
using Sondor.Translations.Options;

namespace Sondor.ResultManager.Tests.Extensions;

/// <summary>
/// Tests for <see cref="ResultManager.Extensions.ServiceCollectionExtensions"/>.
/// </summary>
[TestFixture]
public class ServiceCollectionExtensionsTests
{
    /// <summary>
    /// Ensures that <see cref="ResultManager.Extensions.ServiceCollectionExtensions.AddSondorResultManager"/> works as expected.
    /// </summary>
    [Test]
    public void AddSondorErrors()
    {
        // arrange
        var services = new ServiceCollection()
            .AddLogging()
            .AddHttpContextAccessor()
            .AddTestTranslation(new SondorTranslationOptions
            {
                DefaultCulture = "en",
                SupportedCultures = new LanguageArgs().Cast<string>().ToArray(),
                UseKeyAsDefaultValue = false
            }, "Test:Translation");

        // act
        services.AddSondorResultManager();

        // assert
        var serviceProvider = services.BuildServiceProvider();
        Assert.DoesNotThrow(() => serviceProvider.GetRequiredService<ISondorResultManager>());
    }

    /// <summary>
    /// Ensures that <see cref="ResultManager.Extensions.ServiceCollectionExtensions.AddSondorResultManager{TResultManager}"/> works as expected.
    /// </summary>
    [Test]
    public void AddSondorErrors_typed()
    {
        // arrange
        var services = new ServiceCollection()
            .AddLogging()
            .AddHttpContextAccessor()
            .AddTestTranslation(new SondorTranslationOptions
            {
                DefaultCulture = "en",
                SupportedCultures = new LanguageArgs().Cast<string>().ToArray(),
                UseKeyAsDefaultValue = false
            }, "Test:Translation");

        // act
        services.AddSondorResultManager<TestResultManager>();

        // assert
        var serviceProvider = services.BuildServiceProvider();
        Assert.DoesNotThrow(() => serviceProvider.GetRequiredService<ISondorResultManager>());
    }
}
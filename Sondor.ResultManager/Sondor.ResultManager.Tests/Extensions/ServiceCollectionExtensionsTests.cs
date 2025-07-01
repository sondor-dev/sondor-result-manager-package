using Microsoft.Extensions.DependencyInjection;
using Sondor.ResultManager.Extensions;

namespace Sondor.ResultManager.Tests.Extensions;

/// <summary>
/// Tests for <see cref="ServiceCollectionExtensions"/>.
/// </summary>
[TestFixture]
public class ServiceCollectionExtensionsTests
{
    /// <summary>
    /// Ensures that <see cref="ServiceCollectionExtensions.AddSondorResults"/> works as expected.
    /// </summary>
    [Test]
    public void AddSondorErrors()
    {
        // arrange
        var services = new ServiceCollection();

        // act
        services.AddSondorResultManager();

        // assert
        var serviceProvider = services.BuildServiceProvider();
        Assert.DoesNotThrow(() => serviceProvider.GetRequiredService<ISondorResultManager>());
    }

    /// <summary>
    /// Ensures that <see cref="ServiceCollectionExtensions.AddSondorResults{TResultManager}"/> works as expected.
    /// </summary>
    [Test]
    public void AddSondorErrors_typed()
    {
        // arrange
        var services = new ServiceCollection();

        // act
        services.AddSondorResultManager<TestResultManager>();

        // assert
        var serviceProvider = services.BuildServiceProvider();
        Assert.DoesNotThrow(() => serviceProvider.GetRequiredService<ISondorResultManager>());
    }
}
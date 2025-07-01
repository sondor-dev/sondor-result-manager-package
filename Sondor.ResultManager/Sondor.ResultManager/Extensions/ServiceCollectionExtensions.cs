using Microsoft.Extensions.DependencyInjection;

namespace Sondor.ResultManager.Extensions;

/// <summary>
/// Collection of <see cref="IServiceCollection"/> extensions.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add default <see cref="SondorResultManager"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>Returns the services.</returns>
    public static IServiceCollection AddSondorResultManager(this IServiceCollection services)
    {
        services.AddSingleton<ISondorResultManager, SondorResultManager>();

        return services;
    }

    /// <summary>
    /// Add custom <see cref="ISondorResultManager"/> implementation.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>Returns the services.</returns>
    public static IServiceCollection AddSondorResultManager<TResultManager>(this IServiceCollection services)
        where TResultManager : class, ISondorResultManager
    {
        var interfaceType = typeof(ISondorResultManager);
        var classType = typeof(TResultManager);

        services.AddSingleton(interfaceType, classType);

        return services;
    }
}

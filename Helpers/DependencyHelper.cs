using Autofac.Builder;
using Autofac.ServicesConfigurator.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace Autofac.ServicesConfigurator.Helpers;

public static class DependencyHelper
{
    /// <summary>
    /// Sets the <see cref="ServiceLifetime"/> for registrating service.
    /// </summary>
    /// <typeparam name="TLimit">The type of the limit.</typeparam>
    /// <typeparam name="TActivatorData">The type of the activator data.</typeparam>
    /// <typeparam name="TRegistrationStyle">The type of the registration style.</typeparam>
    /// <param name="registration">The registration.</param>
    /// <param name="lifetime">The lifetime.</param>
    /// <returns></returns>
    public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> SetLifeTime<TLimit, TActivatorData, TRegistrationStyle>(this
        IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registration, ServiceLifetime lifetime)
    {
        switch (lifetime)
        {
            case ServiceLifetime.Singleton:
                registration.SingleInstance();
                break;
            case ServiceLifetime.Transient:
                registration.InstancePerDependency();
                break;
            case ServiceLifetime.Scoped:
                registration.InstancePerLifetimeScope();
                break;
        }

        return registration;
    }

    /// <summary>
    /// Loads services from the configuration.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="logger">The logger.</param>
    public static void LoadWithConfiguration(this ContainerBuilder builder, ILogger logger, string jsonConfigFilePath = "appsettings.json")
    {
        builder.RegisterType<MicroserviceLoader>().As<IMicroserviceLoader>();

        MicroserviceLoader loader = new(logger);
        loader.LoadServices(builder, jsonConfigFilePath);
    }
}
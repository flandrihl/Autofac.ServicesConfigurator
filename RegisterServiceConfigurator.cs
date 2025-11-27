using Autofac.Builder;
using Autofac.ServicesConfigurator.Helpers;
using Autofac.ServicesConfigurator.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NLog;
using System;
using System.Reflection;

namespace Autofac.ServicesConfigurator;

[JsonObject(ItemTypeNameHandling = TypeNameHandling.Auto)]
public class RegisterServiceConfigurator : IRegisterServiceConfigurator
{
    public string Name { get; set; } = string.Empty;

    public string AssemblyPath { get; set; } = string.Empty;

    public string InterfaceType { get; set; } = string.Empty;

    public string ImplementationType { get; set; } = string.Empty;

    public string Lifetime { get; set; } = "Singleton";

    protected ServiceLifetime GetLifeTime()
    {
        if (string.IsNullOrWhiteSpace(Lifetime) || !Enum.TryParse<ServiceLifetime>(Lifetime, out var lifeTime))
            return ServiceLifetime.Singleton;

        return lifeTime;
    }

    public void LoadService(ContainerBuilder builder, ILogger logger)
    {
        try
        {
            // Загрузка сборки
            var assembly = Assembly.LoadFrom(AssemblyPath);

            // Получение типа интерфейса и реализации
            var interfaceType = GetTypeFromString(InterfaceType, assembly);
            var implementationType = GetTypeFromString(ImplementationType, assembly);


            if (interfaceType != null && implementationType != null)
            {
                var lifetime = GetLifeTime();
                Registration(builder, implementationType, interfaceType)
                    .SetLifeTime(lifetime);
                logger.Debug($"Successfully loaded: {Name}");
            }
        }
        catch (Exception ex)
        {
            logger.Fatal($"Failed to load service {Name}: {ex.Message}");
        }
    }

    /// <summary>
    /// Configures the parameters for registrating service.
    /// </summary>
    /// <param name="registration">The registration.</param>
    /// <returns></returns>
    protected virtual IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> ConfigureParameters(
        IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration) => registration;

    private IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> Registration(
        ContainerBuilder builder,
        Type implementationType,
        Type interfaceType)
    {
        var registration = builder
            .RegisterType(implementationType)
            .As(interfaceType);

        return ConfigureParameters(registration);
    }

    /// <summary>
    /// Gets the type from string.
    /// </summary>
    /// <param name="typeString">The type string.</param>
    /// <param name="assembly">The assembly.</param>
    /// <returns></returns>
    private Type GetTypeFromString(string typeString, Assembly assembly)
    {
        try
        {
            // Попытка загрузить тип из указанной сборки
            var typeName = typeString.Split(',')[0].Trim();
            return assembly.GetType(typeName) ?? Type.GetType(typeString);
        }
        catch
        {
            return Type.GetType(typeString);
        }
    }
}
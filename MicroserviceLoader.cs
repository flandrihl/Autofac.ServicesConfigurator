using Autofac.ServicesConfigurator.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;

namespace Autofac.ServicesConfigurator;

public class MicroserviceLoader(ILogger logger) : IMicroserviceLoader
{
    public void LoadServices(ContainerBuilder builder, IConfiguration configuration)
    {
        try
        {
            var servicesSection = configuration.GetSection("Microservices:Services");
            var servicesJson = servicesSection.Get<string>();

            if (string.IsNullOrEmpty(servicesJson)) return;

            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Error = (sender, args) =>
                {
                    // Обработка ошибок десериализации
                    logger.Fatal($"Error deserializing: {args.ErrorContext.Error.Message}");
                    args.ErrorContext.Handled = true;
                }
            };
            var services = JsonConvert
                .DeserializeObject<List<RegisterServiceConfigurator>>(servicesJson, settings) ?? [];

            foreach (var serviceConfig in services)
            {
                try
                {
                    serviceConfig.LoadService(builder, logger);
                }
                catch (Exception ex)
                {
                    logger.Fatal($"Error loading service {serviceConfig.Name}: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            logger.Fatal($"Error in MicroserviceLoader: {ex.Message}");
        }
    }
}
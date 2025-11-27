using Autofac.ServicesConfigurator.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.IO;

namespace Autofac.ServicesConfigurator;

public class MicroserviceLoader(ILogger logger) : IMicroserviceLoader
{
    public void LoadServices(ContainerBuilder builder, string jsonConfigFileName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(jsonConfigFileName))
                throw new ArgumentNullException(nameof(jsonConfigFileName));
            if (!File.Exists(jsonConfigFileName))
                throw new FileNotFoundException("Fail loading gonfiguration", jsonConfigFileName);

            var jsonString = File.ReadAllText(jsonConfigFileName);
            var jsonObject = JObject.Parse(jsonString);

            var servicesArray = jsonObject["Microservices"]?["Services"] as JArray;
            if (servicesArray == null) return;

            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            };

            foreach (var item in servicesArray)
            {
                var serviceConfig = item.ToObject<RegisterServiceConfigurator>(JsonSerializer.Create(settings));
                serviceConfig?.LoadService(builder, logger);
            }
        }
        catch (Exception ex)
        {
            logger.Fatal($"Error in MicroserviceLoader: {ex.Message}");
        }
    }
}
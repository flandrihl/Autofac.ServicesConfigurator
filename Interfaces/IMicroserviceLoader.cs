namespace Autofac.ServicesConfigurator.Interfaces;

public interface IMicroserviceLoader
{
    /// <summary>
    /// Loads the services and configure it.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="configuration">The configuration.</param>
    void LoadServices(ContainerBuilder builder, string jsonConfigFilePath);
}
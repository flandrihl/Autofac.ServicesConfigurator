namespace Autofac.ServicesConfigurator.Interfaces;

public interface IRegisterServiceConfigurator
{
    /// <summary>
    /// Gets the name of configutation service.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    string Name { get; }
    /// <summary>
    /// Gets the assembly path for load a service.
    /// </summary>
    /// <value>
    /// The assembly path.
    /// </value>
    string AssemblyPath { get; }
    /// <summary>
    /// Gets the type of base interface.
    /// </summary>
    /// <value>
    /// The type of the interface.
    /// </value>
    string InterfaceType { get; }
    /// <summary>
    /// Gets the type of the implementation when loading a assembly.
    /// </summary>
    /// <value>
    /// The type of the implementation.
    /// </value>
    string ImplementationType { get; }
    /// <summary>
    /// Gets the lifetime.
    /// </summary>
    /// <value>
    /// The lifetime.
    /// </value>
    string Lifetime { get; }

    /// <summary>
    /// Loads the service.
    /// </summary>
    /// <param name="builder">The builder.</param>
    void LoadService(ContainerBuilder builder, NLog.ILogger logger);
}
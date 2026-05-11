using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace PyTrain.ApiClient;

public static class ServiceCollectionExtensions
{
  /// <summary>
  /// <para>
  ///   Adds services for interacting with the PyTrain API via the custom HTTP API client.
  /// </para>
  /// <para>
  ///   The <see cref="IPyTrainApiClientFactory"/> will be registered as a singleton service,
  ///   which can be used to create instances of <see cref="IPyTrainApi"/>.
  /// </para>
  /// <para>
  ///   The <see cref="IPyTrainApi"/> will be registered as a transient service and can also be injected directly.
  /// </para>
  /// </summary>
  /// <param name="services">
  ///   The <see cref="IServiceCollection"/> to which the services are added.
  /// </param>
  /// <param name="configureOptions">
  ///   The action used to configure the <see cref="PyTrainApiClientOptions"/>.
  /// </param>
  /// <returns>
  ///   The <see cref="IServiceCollection"/> to allow for chaining further calls.
  /// </returns>
  public static IServiceCollection AddPyTrainApiClient(
    this IServiceCollection services,
    Action<PyTrainApiClientOptions> configureOptions)
  {
    // Register and validate options using the options pattern
    services
      .AddOptions<PyTrainApiClientOptions>()
      .Configure(configureOptions)
      .Validate(options => options.BaseUrl is not null, "BaseUrl is required.")
      .ValidateOnStart();

    // Register the factory for the PyTrain API client.
    services
      .AddHttpClient<IPyTrainApi, PyTrainApi>(
      (sp, client) =>
      {
        var options = sp.GetRequiredService<IOptionsMonitor<PyTrainApiClientOptions>>().CurrentValue;
        client.BaseAddress = options.BaseUrl;
        if (!string.IsNullOrWhiteSpace(options.PersonalAccessToken))
        {
          client.DefaultRequestHeaders.Add(PyTrainApiClientOptions.PersonalAccessTokenHeader, options.PersonalAccessToken);
        }
      });
    return services;
  }

  /// <summary>
  /// <para>
  ///   Adds services for interacting with the PyTrain API via the custom HTTP API client.
  /// </para>
  /// <para>
  ///   Configuration is loaded from the specified configuration section.
  /// </para>
  /// <para>
  ///   <see cref="IPyTrainApiClientFactory"/> will be registered as a singleton service,
  ///   which can be used to create instances of <see cref="IPyTrainApi"/>.
  /// </para>
  /// <para>
  ///   The <see cref="IPyTrainApi"/> will be registered as a transient service and can also be injected directly.
  /// </para>
  /// </summary>
  /// <param name="services">
  ///   The <see cref="IServiceCollection"/> to which the services are added.
  /// </param>
  /// <param name="configuration">
  ///   The <see cref="IConfiguration"/> instance to bind options from.
  /// </param>
  /// <param name="configurationSectionName">
  ///   The name of the configuration section containing the <see cref="PyTrainApiClientOptions"/>.
  /// </param>
  /// <returns>
  ///   The <see cref="IServiceCollection"/> to allow for chaining further calls.
  /// </returns>
  public static IServiceCollection AddPyTrainApiClient(
    this IServiceCollection services,
    IConfiguration configuration,
    string configurationSectionName)
  {
    // Register and validate options using the options pattern.
    services
      .AddOptions<PyTrainApiClientOptions>()
      .Bind(configuration.GetSection(configurationSectionName))
      .Validate(options => options.BaseUrl is not null, "BaseUrl is required.")
      .ValidateOnStart();

    // Register the factory for the PyTrain API client.
    services
      .AddHttpClient<IPyTrainApi, PyTrainApi>(
      (sp, client) =>
      {
        var options = sp.GetRequiredService<IOptionsMonitor<PyTrainApiClientOptions>>().CurrentValue;
        client.BaseAddress = options.BaseUrl;
        if (!string.IsNullOrWhiteSpace(options.PersonalAccessToken))
        {
          client.DefaultRequestHeaders.Add(PyTrainApiClientOptions.PersonalAccessTokenHeader, options.PersonalAccessToken);
        }
      });

    return services;
  }

  /// <summary>
  /// <para>
  ///   Adds services for interacting with the PyTrain API via the custom HTTP API client.
  /// </para>
  /// <para>
  ///   Configuration is loaded from the specified configuration section using the builder's <see cref="IHostApplicationBuilder.Configuration"/>.
  /// </para>
  /// <para>
  ///   The <see cref="IPyTrainApiClientFactory"/> will be registered as a singleton service,
  ///   which can be used to create instances of <see cref="IPyTrainApi"/>.
  /// </para>
  /// <para>
  ///   The <see cref="IPyTrainApi"/> will be registered as a transient service and can also be injected directly.
  /// </para>
  /// </summary>
  /// <param name="builder">
  ///   The <see cref="IHostApplicationBuilder"/> to add the services to.
  /// </param>
  /// <param name="configurationSectionName">
  ///   The name of the configuration section containing the <see cref="PyTrainApiClientOptions"/>.
  /// </param>
  /// <returns>
  ///   The <see cref="IHostApplicationBuilder"/> to allow for chaining further calls.
  /// </returns>
  public static IHostApplicationBuilder AddPyTrainApiClient(
    this IHostApplicationBuilder builder,
    string configurationSectionName)
  {
    builder.Services.AddPyTrainApiClient(builder.Configuration, configurationSectionName);
    return builder;
  }
}
using Microsoft.Extensions.DependencyInjection;

namespace PyTrain.ApiClient;

public static class PyTrainApiClientBuilder
{
  private static readonly Lock _servicesLock = new();

  private static IServiceCollection? _serviceCollection;
  private static IServiceProvider? _serviceProvider;

  /// <summary>
  ///   Creates a new instance of <see cref="IPyTrainApi"/>.
  /// </summary>
  /// <remarks>
  /// <para>
  ///   Internally, the <see cref="IHttpClientFactory"/> is used to manage the lifetime of the underlying <see cref="HttpClient"/> instances,
  ///   its message handlers, and associated socket resources.  As such, you don't have to worry about socket exhaustion or other common pitfalls.
  /// </para>
  /// </remarks>
  /// <exception cref="InvalidOperationException"></exception>
  public static IPyTrainApi GetClient()
  {
    if (_serviceProvider is null)
    {
      throw new InvalidOperationException(
        $"The API client builder has not been initialized.  Call {nameof(Initialize)} first.");
    }

    return _serviceProvider.GetRequiredService<IPyTrainApi>();
  }

  public static void Initialize(Action<PyTrainApiClientOptions> configureOptions)
  {
    using var lockScope = _servicesLock.EnterScope();
    if (_serviceProvider is not null)
    {
      return;
    }
    _serviceCollection = new ServiceCollection();
    _serviceCollection.AddPyTrainApiClient(configureOptions);
    _serviceProvider = _serviceCollection.BuildServiceProvider();
  }
}

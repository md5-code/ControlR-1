using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace PyTrain.Libraries.Avalonia.Controls.Snackbar;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddPyTrainSnackbar(this IServiceCollection services)
  {
    return services.AddPyTrainSnackbar(_ => { });
  }

  public static IServiceCollection AddPyTrainSnackbar(
    this IServiceCollection services,
    Action<SnackbarOptions> configureOptions)
  {
    services.AddOptions();
    services.Configure(configureOptions);
    services.TryAddSingleton<ISnackbar, SnackbarService>();
    return services;
  }
}
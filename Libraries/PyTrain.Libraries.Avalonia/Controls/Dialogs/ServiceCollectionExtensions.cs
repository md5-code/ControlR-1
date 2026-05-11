using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace PyTrain.Libraries.Avalonia.Controls.Dialogs;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddPyTrainDialogs(this IServiceCollection services)
  {
    services.TryAddSingleton<IDialogProvider, DialogProvider>();
    return services;
  }
}

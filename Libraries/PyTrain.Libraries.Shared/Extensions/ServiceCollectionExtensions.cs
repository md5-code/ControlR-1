using PyTrain.Libraries.Shared.Services;
using Microsoft.Extensions.DependencyInjection;

namespace PyTrain.Libraries.Shared.Extensions;
public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddLazyInjection(this IServiceCollection services)
  {
    return services.AddTransient(typeof(ILazyInjector<>), typeof(LazyInjector<>));
  }
}
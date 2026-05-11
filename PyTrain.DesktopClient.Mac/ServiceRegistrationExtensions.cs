using PyTrain.DesktopClient.Common.ServiceInterfaces;
using PyTrain.DesktopClient.Common.Services;
using PyTrain.DesktopClient.Mac.Helpers;
using PyTrain.DesktopClient.Mac.Services;
using PyTrain.DesktopClient.ViewModels.Mac;
using PyTrain.Libraries.NativeInterop.Unix;
using PyTrain.Libraries.Serilog;
using PyTrain.Libraries.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PyTrain.DesktopClient.Mac;

public static class ServiceRegistrationExtensions
{
  public static IServiceCollection AddDesktopAppPlatformServices(this IServiceCollection services)
  {
    return services
      .AddSharedPlatformServices()
      .AddSingleton<INavigationItemProvider, MacNavigationItemProvider>()
      .AddSingleton<IRemoteControlHostBuilderFactory, MacRemoteControlHostBuilderFactory>()
      .AddSingleton<IPermissionsViewModelMac, PermissionsViewModelMac>()
      .AddHostedService<RemoteControlPermissionMonitorMac>();
  }

  public static IHostApplicationBuilder AddRemoteControlPlatformServices(this IHostApplicationBuilder builder)
  {
    builder.Services.AddSharedPlatformServices();
    AddInputSimulator(builder.Services);
    AddRemoteControlHostedServices(builder.Services);

    return builder;
  }

  public static IServiceCollection AddSharedPlatformServices(this IServiceCollection services)
  {
    return services
      .AddSingleton<IMacInterop>(provider => new MacInterop(provider.GetRequiredService<ILogger<MacInterop>>()))
      .AddSingleton<IDisplayManager, DisplayManagerMac>()
      .AddSingleton<IDisplayEnumHelperMac, DisplayEnumHelperMac>()
      .AddSingleton<IScreenGrabberFactory, ScreenGrabberFactory<ScreenGrabberMac>>()
      .AddSingleton(provider => provider.GetRequiredService<IScreenGrabberFactory>().GetOrCreateDefault())
      .AddSingleton<IClipboardManager, ClipboardManagerMac>()
      .AddSingleton<ICaptureMetrics, CaptureMetricsMac>()
      .AddSingleton<IFileSystemUnix, FileSystemUnix>();
  }

  private static IServiceCollection AddInputSimulator(IServiceCollection services)
  {
    return services
      .AddSingleton<IInputSimulator, InputSimulatorMac>();
  }

  private static IServiceCollection AddRemoteControlHostedServices(IServiceCollection services)
  {
    return services
      .AddHostedService<ScreenWakerMac>()
      .AddHostedService<CursorWatcherMac>();
  }
}
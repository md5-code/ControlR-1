using PyTrain.DesktopClient.Common.ServiceInterfaces;
using PyTrain.DesktopClient.Common.Services;
using PyTrain.DesktopClient.ViewModels;
using PyTrain.DesktopClient.Windows.Services;
using PyTrain.Libraries.NativeInterop.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PyTrain.DesktopClient.Windows;

public static class ServiceRegistrationExtensions
{
  public static IServiceCollection AddDesktopAppPlatformServices(this IServiceCollection services)
  {
    return services
      .AddSharedPlatformServices()
      .AddSingleton<INavigationItemProvider, WindowsNavigationItemProvider>()
      .AddSingleton<IRemoteControlHostBuilderFactory, WindowsRemoteControlHostBuilderFactory>()
      .AddSingleton<IPermissionsViewModel, PermissionsViewModel>();
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
      .AddSingleton<IWin32Interop, Win32Interop>()
      .AddSingleton<IUrlLauncher, WindowsUrlLauncher>()
      .AddSingleton<ICaptureMetrics, CaptureMetricsWindows>()
      .AddSingleton<IClipboardManager, ClipboardManagerWindows>()
      .AddSingleton<DisplayManagerWindows>()
      .AddSingleton<IDisplayManager>(provider => provider.GetRequiredService<DisplayManagerWindows>())
      .AddSingleton<IDisplayManagerWindows>(provider => provider.GetRequiredService<DisplayManagerWindows>())
      .AddSingleton<IScreenGrabberFactory, ScreenGrabberFactory<ScreenGrabberWindows>>()
      .AddSingleton(provider => provider.GetRequiredService<IScreenGrabberFactory>().GetOrCreateDefault())
      .AddSingleton<IDxOutputDuplicator, DxOutputDuplicator>()
      .AddSingleton<IWindowsMessagePump, WindowsMessagePump>()
      .AddSingleton<IAeroPeekProvider, AeroPeekProvider>();
  }

  private static IServiceCollection AddInputSimulator(IServiceCollection services)
  {
    return services
      .AddSingleton<InputSimulatorWindows>()
      .AddSingleton<IInputSimulator>(provider => provider.GetRequiredService<InputSimulatorWindows>());
  }

  private static IServiceCollection AddRemoteControlHostedServices(IServiceCollection services)
  {
    return services
      .AddHostedService(provider => provider.GetRequiredService<IWindowsMessagePump>())
      .AddHostedService<SystemEventHandler>()
      .AddHostedService<RemoteControlEnvironmentService>()
      .AddHostedService<InputDesktopReporter>()
      .AddHostedService(provider => provider.GetRequiredService<InputSimulatorWindows>())
      .AddHostedService<CursorWatcherWindows>();
  }
}
using Bitbound.SimpleMessenger;
using PyTrain.DesktopClient.Common.Options;
using PyTrain.DesktopClient.Common.ServiceInterfaces;
using PyTrain.DesktopClient.Common.Services;
using PyTrain.DesktopClient.Common.Services.Encoders;
using PyTrain.DesktopClient.Common.State;
using PyTrain.Libraries.Hosting;
using PyTrain.Libraries.Shared.Services;
using PyTrain.Libraries.Shared.Services.Buffers;
using PyTrain.Libraries.Shared.Services.FileSystem;
using PyTrain.Libraries.Shared.Services.Processes;
using PyTrain.Libraries.WebSocketRelay.Client;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PyTrain.DesktopClient.Common;

public static class ServiceRegistrationExtensions
{
  public static IHostApplicationBuilder AddCommonRemoteControlServices(
    this IHostApplicationBuilder builder,
    Action<IHostApplicationBuilder> configureDependencies,
    Action<RemoteControlSessionOptions> configureSessionOptions,
    Action<DesktopClientOptions> configureDesktopClientOptions)
  {
    builder.Configuration.AddEnvironmentVariables();

    configureDependencies(builder);

    builder.Services
      .AddOptions()
      .AddTransient<IHubConnectionBuilder, HubConnectionBuilder>()
      .AddSingleton<IMessenger>(new WeakReferenceMessenger())
      .AddSingleton(TimeProvider.System)
      .AddSingleton<IProcessManager, ProcessManager>()
      .AddSingleton<IFileSystem, FileSystem>()
      .AddSingleton<IImageUtility, ImageUtility>()
      .AddSingleton<IStreamMetrics, StreamMetrics>()
      .AddSingleton<IStreamEncoder, Vp9Encoder>()
      .AddSingleton<IMemoryProvider, MemoryProvider>()
      .AddSingleton<ISystemEnvironment, SystemEnvironment>()
      .AddSingleton<IDesktopRemoteControlStream, DesktopRemoteControlStream>()
      .AddSingleton<IDesktopCapturerFactory, DesktopCapturerFactory>()
      .AddSingleton<IFrameEncoder, SkiaSharpEncoder>()
      .AddSingleton<IDesktopPreviewProvider, DesktopPreviewProvider>()
      .AddSingleton<ISessionConsentService, SessionConsentService>()
      .AddSingleton<IRemoteControlSessionState, RemoteControlSessionState>()
      .AddSingleton<IWaiter, Waiter>()
      .AddTransient<FrameBasedCapturer>()
      .AddTransient<StreamBasedCapturer>()
      .AddHostedService<HostLifetimeEventResponder>()
      .AddHostedService<RemoteControlSessionInitializer>()
      .Configure(configureSessionOptions)
      .Configure(configureDesktopClientOptions);

    return builder;
  }
}
using PyTrain.Agent.Common.Interfaces;
using PyTrain.Agent.Common.Models;
using PyTrain.Agent.Common.Services;
using PyTrain.Agent.Common.Services.Linux;
using PyTrain.Agent.Common.Services.Mac;
using PyTrain.Agent.Common.Services.Windows;
using PyTrain.Libraries.Shared.Services.Buffers;
using PyTrain.Libraries.Shared.Services.Http;
using PyTrain.Libraries.Signalr.Client.Extensions;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PyTrain.Web.ServiceDefaults;
using PyTrain.Agent.Common.Services.Terminal;
using PyTrain.Libraries.NativeInterop.Windows;
using PyTrain.Libraries.Ipc;
using PyTrain.Libraries.NativeInterop.Unix;
using PyTrain.Agent.Common.Services.FileManager;
using PyTrain.Libraries.Api.Contracts.Hubs.Clients;
using PyTrain.Libraries.Shared.Helpers;
using PyTrain.Libraries.Shared.Services.Processes;
using PyTrain.Libraries.Hosting;
using PyTrain.Libraries.Serilog;
using PyTrain.Libraries.Shared.Services.FileSystem;
using PyTrain.Agent.Shared.Interfaces;
using PyTrain.Agent.Shared.Options;
using PyTrain.Agent.Shared.Startup;

namespace PyTrain.Agent.Common.Startup;

public static class HostApplicationBuilderExtensions
{
  public static HostApplicationBuilder AddPyTrainAgent(
    this HostApplicationBuilder builder,
    StartupMode startupMode,
    string? instanceId,
    Uri? serverUri,
    bool loadAppSettings = true)
  {
    builder.AddServiceDefaults(ServiceNames.PyTrainAgent);

    instanceId = instanceId?.SanitizeForFileSystem();
    var services = builder.Services;
    var configuration = builder.Configuration;
    services
      .AddWindowsService(config =>
      {
        config.ServiceName = "PyTrain.Agent";
      })
      .AddSystemd();

    if (!SystemEnvironment.Instance.IsDebug)
    {
      configuration.Sources.Clear();
    }

    builder.Configuration
      .AddInMemoryCollection(new Dictionary<string, string?>
      {
        { $"{InstanceOptions.SectionKey}:{nameof(InstanceOptions.InstanceId)}", instanceId },
        { $"{AgentAppOptions.SectionKey}:{nameof(AgentAppOptions.ServerUri)}", serverUri?.ToString() },
      })
      .AddEnvironmentVariables();

    services
      .AddOptions<AgentAppOptions>()
      .Bind(configuration.GetSection(AgentAppOptions.SectionKey));

    services
      .AddOptions<DeveloperOptions>()
      .Bind(configuration.GetSection(DeveloperOptions.SectionKey));

    services
      .AddOptions<InstanceOptions>()
      .Bind(configuration.GetSection(InstanceOptions.SectionKey));

    var pathProvider = GetTempPathProvider(builder);

    if (loadAppSettings)
    {
      builder.Configuration.AddJsonFile(pathProvider.GetAgentAppSettingsPath(), true, true);
    }

    var appOptions = builder.Configuration
      .GetSection(AgentAppOptions.SectionKey)
      .Get<AgentAppOptions>() ?? new AgentAppOptions();

    services.AddHttpClient<IDownloadsApi, DownloadsApi>(ConfigureHttpClient);
    services.AddPyTrainApiClient(options =>
    {
      if (appOptions.ServerUri is null)
      {
        throw new ArgumentException("ServerUri must be provided in configuration or app settings.");
      }
      options.BaseUrl = appOptions.ServerUri;
    });

    services.AddAgentSharedServices();
    services.AddSingleton<IProcessManager, ProcessManager>();
    services.AddSingleton<ISystemEnvironment>(_ => SystemEnvironment.Instance);
    services.AddSingleton<IFileSystem, FileSystem>();
    services.AddSingleton<IFileManager, FileManager>();
    services.AddTransient<IHubConnectionBuilder, HubConnectionBuilder>();
    services.AddSingleton<ILocalSocketProxy, LocalSocketProxy>();
    services.AddSingleton(WeakReferenceMessenger.Default);
    services.AddSingleton(TimeProvider.System);
    services.AddSingleton<IMemoryProvider, MemoryProvider>();
    services.AddSingleton<IWakeOnLanService, WakeOnLanService>();
    services.AddSingleton<IWaiter, Waiter>();
    services.AddSingleton<IRetryer, Retryer>();
    services.AddSingleton<IEmbeddedResourceAccessor, EmbeddedResourceAccessor>();
    services.AddSingleton<IAgentUpdater, AgentUpdater>();
    services.AddSingleton<ITerminalSessionFactory, TerminalSessionFactory>();
    services.AddSingleton<ITerminalStore, TerminalStore>();
    services.AddSingleton<IIpcServerStore, IpcServerStore>();
    services.AddSingleton<IIpcClientAuthenticator, IpcClientAuthenticator>();
    services.AddSingleton<IAgentHeartbeatTimer, AgentHeartbeatTimer>();
    services.AddPyTrainIpcServer<AgentRpcService>();
    services.AddStronglyTypedSignalrClient<IAgentHub, IAgentHubClient, AgentHubClient>(ServiceLifetime.Singleton);

    if (OperatingSystem.IsWindowsVersionAtLeast(8))
    {
      services.AddSingleton<IWin32Interop, Win32Interop>();
      services.AddSingleton<IDesktopSessionProvider, DesktopSessionProviderWindows>();
      services.AddSingleton<IDeviceInfoProvider, DeviceInfoProviderWin>();
      services.AddSingleton<ICpuUtilizationSampler, CpuUtilizationSamplerWin>();
      services.AddSingleton<IPowerControl, PowerControlWindows>();
      services.AddSingleton<IIpcClientCredentialsProvider, IpcClientCredentialsProviderWindows>();
      services.AddSingleton<IDesktopClientFileVerifier, DesktopClientFileVerifierWin>();
    }
    else if (OperatingSystem.IsLinux())
    {
      services.AddSingleton<IDeviceInfoProvider, DeviceInfoProviderLinux>();
      services.AddSingleton<ICpuUtilizationSampler, CpuUtilizationSampler>();
      services.AddSingleton<IDesktopSessionProvider, DesktopSessionProviderLinux>();
      services.AddSingleton<IPowerControl, PowerControlLinux>();
      services.AddSingleton<IFileSystemUnix, FileSystemUnix>();
      services.AddSingleton<IIpcClientCredentialsProvider, IpcClientCredentialsProviderLinux>();
      services.AddSingleton<IDesktopClientFileVerifier, DesktopClientFileVerifierLinux>();
      services.AddSingleton<IDesktopEnvironmentDetectorAgent, DesktopEnvironmentDetectorAgent>();
    }
    else if (OperatingSystem.IsMacOS())
    {
      services.AddSingleton<IDeviceInfoProvider, DeviceInfoProviderMac>();
      services.AddSingleton<ICpuUtilizationSampler, CpuUtilizationSampler>();
      services.AddSingleton<IDesktopSessionProvider, DesktopSessionProviderMac>();
      services.AddSingleton<IPowerControl, PowerControlMac>();
      services.AddSingleton<IFileSystemUnix, FileSystemUnix>();
      services.AddSingleton<IIpcClientCredentialsProvider, IpcClientCredentialsProviderMac>();
      services.AddSingleton<IDesktopClientFileVerifier, DesktopClientFileVerifierMac>();
    }
    else
    {
      throw new PlatformNotSupportedException();
    }

    // Add services only needed when running.
    if (startupMode == StartupMode.Run)
    {
      services.AddHostedService<DotnetExtractDirectoryCleanupHostedService>();
      services.AddHostedService(s => s.GetRequiredService<IAgentUpdater>());
      services.AddHostedService<IpcServerWatcher>();
      services.AddHostedService<HubConnectionInitializer>();
      services.AddHostedService(x => x.GetRequiredService<IAgentHeartbeatTimer>());
      services.AddHostedService<MessageHandler>();
      services.AddHostedService<HostLifetimeEventResponder>();
      services.AddHostedService(s => s.GetRequiredService<ICpuUtilizationSampler>());

      if (OperatingSystem.IsWindowsVersionAtLeast(8))
      {
        services.AddSingleton<IDesktopClientLaunchTracker, DesktopClientLaunchTracker>();
        services.AddHostedService<DesktopClientWatcherWin>();
        services.AddHostedService<IpcServerInitializerWindows>();
      }
      else if (OperatingSystem.IsMacOS())
      {
        services.AddHostedService<DesktopClientWatcherMac>();
        services.AddHostedService<IpcServerInitializerMac>();
      }
      else if (OperatingSystem.IsLinux())
      {
        services.AddHostedService<DesktopClientWatcherLinux>();
        services.AddHostedService<IpcServerInitializerLinux>();
      }
    }

    builder.BootstrapSerilog(pathProvider.GetAgentLogFilePath(), TimeSpan.FromDays(7));

    return builder;
  }

  private static void ConfigureHttpClient(IServiceProvider provider, HttpClient client)
  {
    var options = provider.GetRequiredService<IOptionsMonitor<AgentAppOptions>>();
    client.BaseAddress = options.CurrentValue.ServerUri;
  }

  private static FileSystemPathProvider GetTempPathProvider(HostApplicationBuilder builder)
  {
    var instanceOptions = builder.Configuration
      .GetSection(InstanceOptions.SectionKey)
      .Get<InstanceOptions>() ?? new InstanceOptions();

    IElevationChecker elevationChecker =
      SystemEnvironment.Instance.IsWindows()
        ? new ElevationCheckerWin()
        : SystemEnvironment.Instance.IsMacOS()
          ? new ElevationCheckerMac()
          : SystemEnvironment.Instance.IsLinux()
            ? new ElevationCheckerLinux()
            : throw new PlatformNotSupportedException();

    return new FileSystemPathProvider(
      SystemEnvironment.Instance,
      elevationChecker,
      new FileSystem(new SerilogLogger<FileSystem>()),
      new OptionsMonitorWrapper<InstanceOptions>(instanceOptions));
  }
}

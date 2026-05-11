using PyTrain.Agent.Shared.Interfaces;
using PyTrain.Agent.Shared.Options;
using PyTrain.Agent.Shared.Services;
using PyTrain.Agent.Shared.Services.Linux;
using PyTrain.Agent.Shared.Services.Mac;
using PyTrain.Agent.Shared.Services.Windows;
using PyTrain.Libraries.NativeInterop.Windows;
using PyTrain.Libraries.Shared.Helpers;
using PyTrain.Libraries.Shared.Services.FileSystem;
using PyTrain.Libraries.Shared.Services.Http;
using PyTrain.Libraries.Shared.Services.Processes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;

namespace PyTrain.Agent.Shared.Startup;

public static class AgentSharedBuilderExtensions
{
  public static IServiceCollection AddAgentSharedServices(this IServiceCollection services)
  {
    services.AddSingleton<IOptionsAccessor, OptionsAccessor>();
    services.AddSingleton<IFileSystemPathProvider, FileSystemPathProvider>();

    if (OperatingSystem.IsWindowsVersionAtLeast(8))
    {
      services.AddSingleton<IAgentInstaller, AgentInstallerWindows>();
      services.AddSingleton<IServiceControl, ServiceControlWindows>();
      services.AddSingleton<IElevationChecker, ElevationCheckerWin>();
      services.AddSingleton<IRegistryAccessor, RegistryAccessor>();
    }
    else if (OperatingSystem.IsLinux())
    {
      services.AddSingleton<IAgentInstaller, AgentInstallerLinux>();
      services.AddSingleton<IElevationChecker, ElevationCheckerLinux>();
      services.AddSingleton<ILoggedInUserProvider, LoggedInUserProviderLinux>();
      services.AddSingleton<IHeadlessServerDetector, HeadlessServerDetector>();
      services.AddSingleton<IServiceControl, ServiceControlLinux>();
    }
    else if (OperatingSystem.IsMacOS())
    {
      services.AddSingleton<IAgentInstaller, AgentInstallerMac>();
      services.AddSingleton<IServiceControl, ServiceControlMac>();
      services.AddSingleton<IElevationChecker, ElevationCheckerMac>();
    }

    return services;
  }

  public static HostApplicationBuilder AddPyTrainInstallerServices(
    this HostApplicationBuilder builder,
    string? instanceId,
    Uri? serverUri,
    bool loadAppSettings)
  {
    instanceId = instanceId?.SanitizeForFileSystem();

    builder.Configuration
      .AddInMemoryCollection(new Dictionary<string, string?>
      {
        { $"{InstanceOptions.SectionKey}:{nameof(InstanceOptions.InstanceId)}", instanceId },
        { $"{AgentAppOptions.SectionKey}:{nameof(AgentAppOptions.ServerUri)}", serverUri?.ToString() },
      })
      .AddEnvironmentVariables();

    var pathProvider = CreatePathProvider(instanceId);

    if (loadAppSettings)
    {
      builder.Configuration.AddJsonFile(pathProvider.GetAgentAppSettingsPath(), optional: true, reloadOnChange: true);
    }

    builder.Services
      .AddOptions<AgentAppOptions>()
      .Bind(builder.Configuration.GetSection(AgentAppOptions.SectionKey));

    builder.Services
      .AddOptions<DeveloperOptions>()
      .Bind(builder.Configuration.GetSection(DeveloperOptions.SectionKey));

    builder.Services
      .AddOptions<InstanceOptions>()
      .Bind(builder.Configuration.GetSection(InstanceOptions.SectionKey));

    var configuredServerUri = builder.Configuration
      .GetSection(AgentAppOptions.SectionKey)
      .Get<AgentAppOptions>()?
      .ServerUri;

    var apiBaseUri = serverUri ?? configuredServerUri ?? throw new InvalidOperationException("Server URI must be provided either through parameters or configuration.");

    builder.Services.AddPyTrainApiClient(options => options.BaseUrl = apiBaseUri);
    builder.Services.AddHttpClient<IDownloadsApi, DownloadsApi>((_, client) => client.BaseAddress = apiBaseUri);

    builder.Services.AddSingleton<ISystemEnvironment>(_ => SystemEnvironment.Instance);
    builder.Services.AddSingleton(TimeProvider.System);
    builder.Services.AddSingleton<IFileSystem, FileSystem>();
    builder.Services.AddSingleton<IProcessManager, ProcessManager>();
    builder.Services.AddSingleton<IRetryer, Retryer>();
    builder.Services.AddSingleton<IEmbeddedResourceAccessor, EmbeddedResourceAccessor>();
    builder.Services.AddSingleton<ICpuUtilizationSampler, CpuUtilizationSampler>();
    builder.Services.AddAgentSharedServices();

    if (OperatingSystem.IsWindowsVersionAtLeast(8, 0))
    {
      builder.Services.AddSingleton<IWin32Interop, Win32Interop>();
      builder.Services.AddSingleton<IDeviceInfoProvider, DeviceInfoProviderWin>();
    }
    else if (OperatingSystem.IsLinux())
    {
      builder.Services.AddSingleton<IDeviceInfoProvider, DeviceInfoProviderLinux>();
    }
    else if (OperatingSystem.IsMacOS())
    {
      builder.Services.AddSingleton<IDeviceInfoProvider, DeviceInfoProviderMac>();
    }
    else
    {
      throw new PlatformNotSupportedException("Unsupported operating system.");
    }

    return builder;
  }

  private static IElevationChecker CreateElevationChecker()
  {
    if (OperatingSystem.IsWindows())
    {
      return new ElevationCheckerWin();
    }

    if (OperatingSystem.IsMacOS())
    {
      return new ElevationCheckerMac();
    }

    if (OperatingSystem.IsLinux())
    {
      return new ElevationCheckerLinux();
    }

    throw new PlatformNotSupportedException();
  }

  private static FileSystemPathProvider CreatePathProvider(string? instanceId)
  {
    var instanceOptions = new InstanceOptions { InstanceId = instanceId };

    return new FileSystemPathProvider(
      SystemEnvironment.Instance,
      CreateElevationChecker(),
      new FileSystem(NullLogger<FileSystem>.Instance),
      new OptionsMonitorWrapper<InstanceOptions>(instanceOptions));
  }
}

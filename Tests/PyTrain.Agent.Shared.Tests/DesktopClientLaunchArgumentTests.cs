using System.Reflection;
using System.Runtime.Versioning;
using PyTrain.Agent.Shared.Interfaces;
using PyTrain.Agent.Shared.Options;
using PyTrain.Agent.Shared.Services;
using PyTrain.Agent.Shared.Services.Linux;
using PyTrain.Agent.Shared.Services.Mac;
using PyTrain.ApiClient;
using PyTrain.Libraries.Shared.Services;
using PyTrain.Libraries.Shared.Services.FileSystem;
using PyTrain.Libraries.Shared.Services.Processes;
using PyTrain.Libraries.TestingUtilities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace PyTrain.Agent.Shared.Tests;

public class DesktopClientLaunchArgumentTests
{
  [LinuxOnlyFact]
  [SupportedOSPlatform("linux")]
  public async Task GetDesktopServiceFile_WhenInstanceIdMissing_OmitsInstanceIdArgument()
  {
    var embeddedResources = new Mock<IEmbeddedResourceAccessor>();
    embeddedResources
      .Setup(x => x.GetResourceAsString(It.IsAny<Assembly>(), "pytrain.desktop.service"))
      .ReturnsAsync("ExecStart={{INSTALL_DIRECTORY}}/DesktopClient/PyTrain.DesktopClient{{INSTANCE_ARGS}}");

    var sut = CreateLinuxInstaller(embeddedResources.Object, instanceId: null);

    var result = await sut.GetDesktopServiceFile();

    Assert.DoesNotContain("--instance-id", result, StringComparison.Ordinal);
    Assert.DoesNotContain("{{INSTANCE_ARGS}}", result, StringComparison.Ordinal);
  }

  [MacOnlyFact]
  [SupportedOSPlatform("macos")]
  public async Task GetLaunchAgentFile_WhenInstanceIdMissing_OmitsInstanceIdArgument()
  {
    var template = """
      <array>
        <string>{{DESKTOP_EXECUTABLE_PATH}}</string>
        <string>--instance-id</string>
        <string>{{INSTANCE_ID}}</string>
      </array>
      """;
    var embeddedResources = new Mock<IEmbeddedResourceAccessor>();
    embeddedResources
      .Setup(x => x.GetResourceAsString(It.IsAny<Assembly>(), "LaunchAgent.plist"))
      .ReturnsAsync(template);

    var sut = CreateMacInstaller(embeddedResources.Object, instanceId: null);

    var result = await sut.GetLaunchAgentFile();

    Assert.DoesNotContain("--instance-id", result, StringComparison.Ordinal);
    Assert.DoesNotContain("{{INSTANCE_ID}}", result, StringComparison.Ordinal);
  }

  [SupportedOSPlatform("linux")]
  private static AgentInstallerLinux CreateLinuxInstaller(IEmbeddedResourceAccessor embeddedResources, string? instanceId)
  {
    return new AgentInstallerLinux(
      Mock.Of<IHostApplicationLifetime>(),
      Mock.Of<IFileSystem>(),
      Mock.Of<IFileSystemPathProvider>(),
      Mock.Of<IProcessManager>(),
      Mock.Of<ISystemEnvironment>(),
      Mock.Of<IPyTrainApi>(),
      Mock.Of<IDeviceInfoProvider>(),
      Mock.Of<IRetryer>(),
      Mock.Of<IOptionsAccessor>(),
      Mock.Of<IElevationChecker>(),
      Mock.Of<IServiceControl>(),
      embeddedResources,
      Mock.Of<IOptionsMonitor<AgentAppOptions>>(),
      Microsoft.Extensions.Options.Options.Create(new InstanceOptions { InstanceId = instanceId }),
      NullLogger<AgentInstallerLinux>.Instance);
  }

  [SupportedOSPlatform("macos")]
  private static AgentInstallerMac CreateMacInstaller(IEmbeddedResourceAccessor embeddedResources, string? instanceId)
  {
    var fileSystem = new Mock<IFileSystem>();
    fileSystem
      .Setup(x => x.JoinPaths('/', It.IsAny<string[]>()))
      .Returns<char, string[]>((separator, segments) => string.Join(separator, segments));

    return new AgentInstallerMac(
      fileSystem.Object,
      Mock.Of<IFileSystemPathProvider>(),
      Mock.Of<IServiceControl>(),
      Mock.Of<IRetryer>(),
      Mock.Of<IPyTrainApi>(),
      embeddedResources,
      Mock.Of<IDeviceInfoProvider>(),
      Mock.Of<IOptionsAccessor>(),
      Mock.Of<IProcessManager>(),
      Mock.Of<ISystemEnvironment>(),
      Mock.Of<IOptionsMonitor<AgentAppOptions>>(),
        Microsoft.Extensions.Options.Options.Create(new InstanceOptions { InstanceId = instanceId }),
      NullLogger<AgentInstallerMac>.Instance);
  }


}
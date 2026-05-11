using PyTrain.Agent.Shared.Options;
using PyTrain.Agent.Shared.Services;
using PyTrain.Agent.Common.Services;
using PyTrain.ApiClient;
using PyTrain.Libraries.Api.Contracts.Dtos;
using PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;
using PyTrain.Libraries.Api.Contracts.Enums;
using PyTrain.Libraries.Shared.Primitives;
using PyTrain.Libraries.Shared.Services;
using PyTrain.Libraries.Shared.Services.Http;
using PyTrain.Libraries.Shared.Services.Processes;
using PyTrain.Libraries.TestingUtilities.FileSystem;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using System.Diagnostics;
using System.Security.Cryptography;

namespace PyTrain.Agent.Common.Tests;

public class AgentUpdaterTests
{
  private static readonly Uri _serverUri = new("https://pytrain.example/");
  private static readonly Guid _tenantId = Guid.Parse("11111111-1111-1111-1111-111111111111");

  [Fact]
  public async Task CheckForUpdate_OnMac_BootstrapsOneShotLaunchDaemon()
  {
    var fixture = new AgentUpdaterFixture();
    fixture.FileSystem.AddFile(fixture.BundleHashPath, "OLD_HASH");
    fixture.SystemEnvironment
      .SetupGet(x => x.Platform)
      .Returns(SystemPlatform.MacOs);
    fixture.SystemEnvironment
      .SetupGet(x => x.Runtime)
      .Returns(RuntimeId.MacOsArm64);

    var installerBytes = new byte[] { 1, 2, 3, 4, 5 };
    var installerSha256 = Convert.ToHexString(SHA256.HashData(installerBytes));
    var downloadedInstallerPath = string.Empty;
    var process = new Mock<IProcess>();
    process
      .Setup(x => x.WaitForExitAsync(It.IsAny<CancellationToken>()))
      .Returns(Task.CompletedTask);

    fixture.AgentUpdateApi
      .Setup(x => x.GetBundleMetadata(RuntimeId.MacOsArm64, It.IsAny<CancellationToken>()))
      .ReturnsAsync(ApiResult.Ok(new BundleMetadataDto
      {
        BundleDownloadUrl = "/downloads/osx-arm64/PyTrain.Agent.bundle.zip",
        BundleSha256 = "NEW_HASH",
        InstallerDownloadUrl = "/downloads/osx-arm64/PyTrain.Agent.Installer",
        InstallerSha256 = installerSha256,
        Runtime = RuntimeId.MacOsArm64,
        Version = Version.Parse("1.2.3")
      }));

    fixture.DownloadsApi
      .Setup(x => x.DownloadFile("/downloads/osx-arm64/PyTrain.Agent.Installer", It.IsAny<string>(), It.IsAny<CancellationToken>()))
      .Returns<string, string, CancellationToken>((_, destinationPath, _) =>
      {
        downloadedInstallerPath = destinationPath;
        fixture.FileSystem.AddFile(destinationPath, installerBytes);
        return Task.FromResult(Result.Ok());
      });

    fixture.ProcessManager
      .Setup(x => x.Start("sudo", It.IsAny<string>()))
      .Returns(process.Object);

    var launchctlStartInfos = new List<ProcessStartInfo>();
    fixture.ProcessManager
      .Setup(x => x.StartAndWaitForExit(It.IsAny<ProcessStartInfo>(), It.IsAny<TimeSpan>()))
      .Returns<ProcessStartInfo, TimeSpan>((startInfo, _) =>
      {
        launchctlStartInfos.Add(startInfo);
        return Task.FromResult(0);
      });

    var updater = fixture.CreateUpdater();

    await updater.CheckForUpdate(force: true, cancellationToken: TestContext.Current.CancellationToken);

    fixture.ProcessManager.Verify(
      x => x.Start("sudo", It.Is<string>(args => args.Contains("chmod +x", StringComparison.Ordinal))),
      Times.Once);
    Assert.Equal(3, launchctlStartInfos.Count);
    Assert.Equal("sudo", launchctlStartInfos[0].FileName);
    Assert.Equal("launchctl bootout system/app.pytrain.agent.installer.instance-1", string.Join(" ", launchctlStartInfos[0].ArgumentList));
    Assert.Equal("sudo", launchctlStartInfos[1].FileName);
    var expectedInstallerPath = Path.Combine(Path.GetTempPath(), "PyTrain_Update", "instance-1", "PyTrain.Agent.Installer");
    Assert.Equal(expectedInstallerPath, downloadedInstallerPath);
    var expectedPlistPath = "/Library/LaunchDaemons/app.pytrain.agent.installer.instance-1.plist";
    Assert.Equal(
      $"launchctl bootstrap system {expectedPlistPath}",
      string.Join(" ", launchctlStartInfos[1].ArgumentList));
    Assert.Equal("sudo", launchctlStartInfos[2].FileName);
    Assert.Equal(
      "launchctl kickstart -k system/app.pytrain.agent.installer.instance-1",
      string.Join(" ", launchctlStartInfos[2].ArgumentList));
  }

  [Fact]
  public async Task CheckForUpdate_WhenInstalledBundleHashDiffers_DownloadsAndLaunchesInstaller()
  {
    var fixture = new AgentUpdaterFixture();
    fixture.FileSystem.AddFile(fixture.BundleHashPath, "OLD_HASH");

    var installerBytes = new byte[] { 1, 2, 3, 4, 5 };
    var installerSha256 = Convert.ToHexString(SHA256.HashData(installerBytes));
    var downloadedInstallerPath = string.Empty;
    var launchedInstallerPath = string.Empty;
    var launchedInstallerArguments = string.Empty;
    var launchedProcess = new Mock<IProcess>();
    launchedProcess
      .Setup(x => x.WaitForExitAsync(It.IsAny<CancellationToken>()))
      .Returns(Task.CompletedTask);

    fixture.AgentUpdateApi
      .Setup(x => x.GetBundleMetadata(RuntimeId.WinX64, It.IsAny<CancellationToken>()))
      .ReturnsAsync(ApiResult.Ok(new BundleMetadataDto
      {
        BundleDownloadUrl = "/downloads/win-x64/PyTrain.Agent.bundle.zip",
        BundleSha256 = "NEW_HASH",
        InstallerDownloadUrl = "/downloads/win-x64/PyTrain.Agent.Installer.exe",
        InstallerSha256 = installerSha256,
        Runtime = RuntimeId.WinX64,
        Version = Version.Parse("1.2.3")
      }));

    fixture.DownloadsApi
      .Setup(x => x.DownloadFile("/downloads/win-x64/PyTrain.Agent.Installer.exe", It.IsAny<string>(), It.IsAny<CancellationToken>()))
      .Returns<string, string, CancellationToken>((_, destinationPath, _) =>
      {
        downloadedInstallerPath = destinationPath;
        fixture.FileSystem.AddFile(destinationPath, installerBytes);
        return Task.FromResult(Result.Ok());
      });

    fixture.ProcessManager
      .Setup(x => x.Start(It.IsAny<string>(), It.IsAny<string>()))
      .Returns<string, string>((fileName, arguments) =>
      {
        launchedInstallerPath = fileName;
        launchedInstallerArguments = arguments;
        return launchedProcess.Object;
      });

    var updater = fixture.CreateUpdater();

    await updater.CheckForUpdate(force: true, cancellationToken: TestContext.Current.CancellationToken);

    Assert.EndsWith("PyTrain.Agent.Installer.exe", downloadedInstallerPath, StringComparison.OrdinalIgnoreCase);
    Assert.Equal(downloadedInstallerPath, launchedInstallerPath);
    Assert.Contains("install", launchedInstallerArguments, StringComparison.Ordinal);
    Assert.Contains("--server-uri", launchedInstallerArguments, StringComparison.Ordinal);
    Assert.Contains("\"https://pytrain.example/\"", launchedInstallerArguments, StringComparison.Ordinal);
    Assert.Contains("--tenant-id", launchedInstallerArguments, StringComparison.Ordinal);
    Assert.Contains(_tenantId.ToString(), launchedInstallerArguments, StringComparison.Ordinal);
    Assert.Contains("--instance-id", launchedInstallerArguments, StringComparison.Ordinal);
    Assert.Contains("\"instance-1\"", launchedInstallerArguments, StringComparison.Ordinal);
  }

  [Fact]
  public async Task CheckForUpdate_WhenInstalledBundleHashMatches_DoesNotDownloadOrLaunchInstaller()
  {
    var fixture = new AgentUpdaterFixture();
    fixture.FileSystem.AddFile(fixture.BundleHashPath, "ABC123");

    fixture.AgentUpdateApi
      .Setup(x => x.GetBundleMetadata(RuntimeId.WinX64, It.IsAny<CancellationToken>()))
      .ReturnsAsync(ApiResult.Ok(new BundleMetadataDto
      {
        BundleDownloadUrl = "/downloads/win-x64/PyTrain.Agent.bundle.zip",
        BundleSha256 = "ABC123",
        InstallerDownloadUrl = "/downloads/win-x64/PyTrain.Agent.Installer.exe",
        InstallerSha256 = "DEF456",
        Runtime = RuntimeId.WinX64,
        Version = Version.Parse("1.2.3")
      }));

    var updater = fixture.CreateUpdater();

    await updater.CheckForUpdate(force: true, cancellationToken: TestContext.Current.CancellationToken);

    fixture.DownloadsApi.Verify(
      x => x.DownloadFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
      Times.Never);
    fixture.ProcessManager.Verify(
      x => x.Start(It.IsAny<string>(), It.IsAny<string>()),
      Times.Never);
    fixture.AgentUpdateApi.Verify(
      x => x.GetBundleMetadata(RuntimeId.WinX64, It.IsAny<CancellationToken>()),
      Times.Once);
  }

  private sealed class AgentUpdaterFixture
  {
    public AgentUpdaterFixture()
    {
      PyTrainApi
        .SetupGet(x => x.AgentUpdate)
        .Returns(AgentUpdateApi.Object);

      HostApplicationLifetime
        .SetupGet(x => x.ApplicationStopping)
        .Returns(CancellationToken.None);

      SettingsProvider
        .SetupGet(x => x.DisableAutoUpdate)
        .Returns(false);
      SettingsProvider
        .SetupGet(x => x.ServerUri)
        .Returns(_serverUri);
      SettingsProvider
        .Setup(x => x.GetRequiredTenantId())
        .Returns(_tenantId);

      SystemEnvironment
        .SetupGet(x => x.Runtime)
        .Returns(RuntimeId.WinX64);
      SystemEnvironment
        .SetupGet(x => x.Platform)
        .Returns(SystemPlatform.Windows);

      PathProvider
        .Setup(x => x.GetBundleHashFilePath())
        .Returns(BundleHashPath);
    }

    public Mock<IAgentUpdateApi> AgentUpdateApi { get; } = new();
    public string BundleHashPath { get; } = @"C:\PyTrain\.pytrain-bundle.sha256";
    public Mock<IPyTrainApi> PyTrainApi { get; } = new();
    public Mock<IDownloadsApi> DownloadsApi { get; } = new();
    public FakeFileSystem FileSystem { get; } = new('\\');
    public Mock<IHostApplicationLifetime> HostApplicationLifetime { get; } = new();
    public Mock<IFileSystemPathProvider> PathProvider { get; } = new();
    public Mock<IProcessManager> ProcessManager { get; } = new();
    public Mock<IOptionsAccessor> SettingsProvider { get; } = new();
    public Mock<ISystemEnvironment> SystemEnvironment { get; } = new();

    public AgentUpdater CreateUpdater()
    {
      return new AgentUpdater(
        TimeProvider.System,
        PyTrainApi.Object,
        DownloadsApi.Object,
        FileSystem,
        PathProvider.Object,
        ProcessManager.Object,
        SystemEnvironment.Object,
        SettingsProvider.Object,
        HostApplicationLifetime.Object,
        Options.Create(new InstanceOptions { InstanceId = "instance-1" }),
        NullLogger<AgentUpdater>.Instance);
    }

    private sealed class NoopDisposable : IDisposable
    {
      public void Dispose()
      {
      }
    }
  }
}

using PyTrain.Agent.Shared.Interfaces;
using PyTrain.Agent.Shared.Options;
using PyTrain.Agent.Shared.Services;
using PyTrain.Libraries.Api.Contracts.Enums;
using PyTrain.Libraries.Shared.Helpers;
using PyTrain.Libraries.Shared.Services;
using PyTrain.Libraries.TestingUtilities.FileSystem;
using Moq;

namespace PyTrain.Agent.Common.Tests;

public class FileSystemPathProviderTests(ITestOutputHelper testOutputHelper)
{
  private readonly ITestOutputHelper _testOutputHelper = testOutputHelper;

  private Mock<IElevationChecker> _elevationChecker = null!;
  private FakeFileSystem _fileSystem = null!;
  private FileSystemPathProvider _pathProvider = null!;
  private Mock<ISystemEnvironment> _systemEnvironment = null!;

  [Theory]
  [InlineData(SystemPlatform.Windows, null, false, false, @"C:\ProgramData\PyTrain\default\appsettings.json")]
  [InlineData(SystemPlatform.Windows, "localhost", false, false, @"C:\ProgramData\PyTrain\localhost\appsettings.json")]
  [InlineData(SystemPlatform.Windows, "pytrain.test.com", false, true, @"C:\ProgramData\PyTrain\Debug\pytrain.test.com\appsettings.json")]
  [InlineData(SystemPlatform.Linux, null, false, false, "/home/testuser/.pytrain/default/appsettings.json")]
  [InlineData(SystemPlatform.Linux, "localhost", false, false, "/home/testuser/.pytrain/localhost/appsettings.json")]
  [InlineData(SystemPlatform.Linux, "pytrain.test.com", true, false, "/etc/pytrain/pytrain.test.com/appsettings.json")]
  [InlineData(SystemPlatform.MacOs, null, false, false, "/Users/testuser/.pytrain/default/appsettings.json")]
  [InlineData(SystemPlatform.MacOs, "localhost", false, false, "/Users/testuser/.pytrain/localhost/appsettings.json")]
  [InlineData(SystemPlatform.MacOs, "pytrain.test.com", true, false, "/etc/pytrain/pytrain.test.com/appsettings.json")]
  public void GetAgentAppSettingsPath_ReturnsCorrectPath(
    SystemPlatform platform,
    string? instanceId,
    bool isElevated,
    bool isDebug,
    string expectedPath)
  {
    Setup(platform, instanceId, isElevated, isDebug);

    var result = _pathProvider.GetAgentAppSettingsPath();
    Assert.Equal(expectedPath, result);
    Assert.EndsWith("appsettings.json", result);
  }

  [Theory]
  [InlineData(SystemPlatform.Windows, null, false, false, @"C:\ProgramData\PyTrain\default\Logs\PyTrain.Agent\LogFile.log")]
  [InlineData(SystemPlatform.Windows, "localhost", false, false, @"C:\ProgramData\PyTrain\localhost\Logs\PyTrain.Agent\LogFile.log")]
  [InlineData(SystemPlatform.Linux, null, false, false, "/home/testuser/.pytrain/default/logs/PyTrain.Agent/LogFile.log")]
  [InlineData(SystemPlatform.Linux, "pytrain.test.com", true, false, "/var/log/pytrain/pytrain.test.com/PyTrain.Agent/LogFile.log")]
  [InlineData(SystemPlatform.MacOs, "localhost", false, false, "/Users/testuser/.pytrain/localhost/logs/PyTrain.Agent/LogFile.log")]
  public void GetAgentLogFilePath_AppendsLogFileName(
    SystemPlatform platform,
    string? instanceId,
    bool isElevated,
    bool isDebug,
    string expectedPath)
  {
    Setup(platform, instanceId, isElevated, isDebug);

    var result = _pathProvider.GetAgentLogFilePath();

    Assert.Equal(expectedPath, result);
  }

  [Theory]
  [InlineData(SystemPlatform.Windows, null, false, false, @"C:\ProgramData\PyTrain\default\Logs\PyTrain.Agent")]
  [InlineData(SystemPlatform.Windows, "localhost", false, true, @"C:\ProgramData\PyTrain\Debug\localhost\Logs\PyTrain.Agent")]
  [InlineData(SystemPlatform.Linux, null, false, false, "/home/testuser/.pytrain/default/logs/PyTrain.Agent")]
  [InlineData(SystemPlatform.Linux, "pytrain.test.com", true, false, "/var/log/pytrain/pytrain.test.com/PyTrain.Agent")]
  [InlineData(SystemPlatform.MacOs, "localhost", false, false, "/Users/testuser/.pytrain/localhost/logs/PyTrain.Agent")]
  public void GetAgentLogsDirectoryPath_ReturnsCorrectStructure(
    SystemPlatform platform,
    string? instanceId,
    bool isElevated,
    bool isDebug,
    string expectedPath)
  {
    Setup(platform, instanceId, isElevated, isDebug);

    var result = _pathProvider.GetAgentLogsDirectoryPath();

    Assert.Equal(expectedPath, result);
  }

  [Theory]
  [InlineData(SystemPlatform.Windows, null, false, false, @"C:\ProgramData\PyTrain\default\.pytrain-bundle.sha256")]
  [InlineData(SystemPlatform.Windows, "localhost", false, false, @"C:\ProgramData\PyTrain\localhost\.pytrain-bundle.sha256")]
  [InlineData(SystemPlatform.Windows, "pytrain.test.com", false, true, @"C:\ProgramData\PyTrain\Debug\pytrain.test.com\.pytrain-bundle.sha256")]
  [InlineData(SystemPlatform.Linux, null, false, false, "/home/testuser/.pytrain/default/.pytrain-bundle.sha256")]
  [InlineData(SystemPlatform.Linux, "localhost", false, false, "/home/testuser/.pytrain/localhost/.pytrain-bundle.sha256")]
  [InlineData(SystemPlatform.Linux, "pytrain.test.com", true, false, "/etc/pytrain/pytrain.test.com/.pytrain-bundle.sha256")]
  [InlineData(SystemPlatform.MacOs, null, false, false, "/Users/testuser/.pytrain/default/.pytrain-bundle.sha256")]
  [InlineData(SystemPlatform.MacOs, "localhost", false, false, "/Users/testuser/.pytrain/localhost/.pytrain-bundle.sha256")]
  [InlineData(SystemPlatform.MacOs, "pytrain.test.com", true, false, "/etc/pytrain/pytrain.test.com/.pytrain-bundle.sha256")]
  public void GetBundleHashFilePath_ReturnsSettingsDirectoryPath(
    SystemPlatform platform,
    string? instanceId,
    bool isElevated,
    bool isDebug,
    string expectedPath)
  {
    Setup(platform, instanceId, isElevated, isDebug);

    var result = _pathProvider.GetBundleHashFilePath();

    Assert.Equal(expectedPath, result);
  }

  [Theory]
  [InlineData(null, "/Applications/PyTrain.app")]
  [InlineData("pytrain.test.com", "/Applications/PyTrain.pytrain.test.com.app")]
  public void GetBundleRootDirectory_MacOs_ReturnsInstalledAppBundlePath(string? instanceId, string expectedPath)
  {
    Setup(SystemPlatform.MacOs, instanceId);

    var result = _pathProvider.GetBundleRootDirectory();

    Assert.Equal(expectedPath, result);
  }

  [Theory]
  [InlineData(null, "/Applications/PyTrain.app/Contents/MacOS/PyTrain.DesktopClient")]
  [InlineData("pytrain.test.com", "/Applications/PyTrain.pytrain.test.com.app/Contents/MacOS/PyTrain.DesktopClient")]
  public void GetDesktopExecutablePath_MacOs_ReturnsInstalledAppExecutablePath(string? instanceId, string expectedPath)
  {
    Setup(SystemPlatform.MacOs, instanceId);

    var result = _pathProvider.GetDesktopExecutablePath();

    Assert.Equal(expectedPath, result);
  }

  [Theory]
  [InlineData(SystemPlatform.Windows, null, false, false, @"C:\ProgramData\PyTrain\default\Logs\PyTrain.Agent.Installer\LogFile.log")]
  [InlineData(SystemPlatform.Windows, "localhost", false, false, @"C:\ProgramData\PyTrain\localhost\Logs\PyTrain.Agent.Installer\LogFile.log")]
  [InlineData(SystemPlatform.Linux, null, false, false, "/home/testuser/.pytrain/default/logs/PyTrain.Agent.Installer/LogFile.log")]
  [InlineData(SystemPlatform.Linux, "pytrain.test.com", true, false, "/var/log/pytrain/pytrain.test.com/PyTrain.Agent.Installer/LogFile.log")]
  [InlineData(SystemPlatform.MacOs, "localhost", false, false, "/Users/testuser/.pytrain/localhost/logs/PyTrain.Agent.Installer/LogFile.log")]
  public void GetInstallerLogFilePath_AppendsLogFileName(
    SystemPlatform platform,
    string? instanceId,
    bool isElevated,
    bool isDebug,
    string expectedPath)
  {
    Setup(platform, instanceId, isElevated, isDebug);

    var result = _pathProvider.GetInstallerLogFilePath();

    Assert.Equal(expectedPath, result);
  }

  [Theory]
  [InlineData(SystemPlatform.Linux, null, "/var/log/pytrain/default/PyTrain.DesktopClient")]
  [InlineData(SystemPlatform.Linux, "localhost", "/var/log/pytrain/localhost/PyTrain.DesktopClient")]
  [InlineData(SystemPlatform.MacOs, "pytrain.test.com", "/var/log/pytrain/pytrain.test.com/PyTrain.DesktopClient")]
  public void GetUnixDesktopClientLogsDirectoryForRoot_ReturnsCorrectStructure(
    SystemPlatform platform,
    string? instanceId,
    string expectedPath)
  {
    Setup(platform, instanceId);

    var result = _pathProvider.GetUnixDesktopClientLogsDirectoryForRoot();

    Assert.Equal(expectedPath, result);
  }

  [Theory]
  [InlineData(SystemPlatform.Linux, "testuser", null, "/home/testuser/.pytrain/default/logs/PyTrain.DesktopClient")]
  [InlineData(SystemPlatform.Linux, "alice", "localhost", "/home/alice/.pytrain/localhost/logs/PyTrain.DesktopClient")]
  [InlineData(SystemPlatform.MacOs, "bob", "pytrain.test.com", "/Users/bob/.pytrain/pytrain.test.com/logs/PyTrain.DesktopClient")]
  public void GetUnixDesktopClientLogsDirectory_ReturnsCorrectStructure(
    SystemPlatform platform,
    string username,
    string? instanceId,
    string expectedPath)
  {
    Setup(platform, instanceId);

    var result = _pathProvider.GetUnixDesktopClientLogsDirectory(username);

    Assert.Equal(expectedPath, result);
  }

  [Theory]
  [InlineData(SystemPlatform.Linux, null)]
  [InlineData(SystemPlatform.MacOs, "")]
  public void GetUnixDesktopClientLogsDirectory_ThrowsOnEmptyUsername(SystemPlatform platform, string? username)
  {
    Setup(platform, null);

    Assert.Throws<ArgumentException>(() =>
      _pathProvider.GetUnixDesktopClientLogsDirectory(username!));
  }

  [Fact]
  public void GetUnixDesktopClientLogsDirectory_ThrowsOnWindows()
  {
    Setup(SystemPlatform.Windows, null);

    Assert.Throws<PlatformNotSupportedException>(() =>
      _pathProvider.GetUnixDesktopClientLogsDirectory("testuser"));
  }

  [Theory]
  [InlineData(false, null, @"C:\ProgramData\PyTrain\default\Logs\PyTrain.DesktopClient")]
  [InlineData(false, "localhost", @"C:\ProgramData\PyTrain\localhost\Logs\PyTrain.DesktopClient")]
  [InlineData(true, "pytrain.test.com", @"C:\ProgramData\PyTrain\Debug\pytrain.test.com\Logs\PyTrain.DesktopClient")]
  public void GetWindowsDesktopClientLogsDirectory_ReturnsCorrectStructure(
    bool isDebug,
    string? instanceId,
    string expectedPath)
  {
    Setup(SystemPlatform.Windows, instanceId, false, isDebug);

    var result = _pathProvider.GetWindowsDesktopClientLogsDirectory();

    Assert.Equal(expectedPath, result);
  }

  [Fact]
  public void GetWindowsDesktopClientLogsDirectory_ThrowsOnNonWindows()
  {
    Setup(SystemPlatform.Linux, null);

    Assert.Throws<PlatformNotSupportedException>(() =>
      _pathProvider.GetWindowsDesktopClientLogsDirectory());
  }

  private void Setup(SystemPlatform platform, string? instanceId, bool isElevated = false, bool isDebug = false)
  {
    _systemEnvironment = new Mock<ISystemEnvironment>();
    _systemEnvironment.Setup(x => x.Platform).Returns(platform);
    _systemEnvironment.Setup(x => x.IsDebug).Returns(isDebug);
    _systemEnvironment.Setup(x => x.IsWindows()).Returns(platform == SystemPlatform.Windows);
    _systemEnvironment.Setup(x => x.IsLinux()).Returns(platform == SystemPlatform.Linux);
    _systemEnvironment.Setup(x => x.IsMacOS()).Returns(platform == SystemPlatform.MacOs);

    switch (platform)
    {
      case SystemPlatform.Windows:
        _systemEnvironment
          .Setup(x => x.GetProfileDirectory())
          .Returns(@"C:\Users\TestUser");

        _systemEnvironment.Setup(x => x.GetCommonApplicationDataDirectory())
          .Returns(@"C:\ProgramData");
        break;
      case SystemPlatform.Linux:
        if (isElevated)
        {
          _systemEnvironment
            .Setup(x => x.GetProfileDirectory())
            .Returns("/root");
        }
        else
        {
          _systemEnvironment
            .Setup(x => x.GetProfileDirectory())
            .Returns("/home/testuser");
        }
        break;
      case SystemPlatform.MacOs:
        if (isElevated)
        {
          _systemEnvironment
            .Setup(x => x.GetProfileDirectory())
            .Returns("/var/root");
        }
        else
        {
          _systemEnvironment
            .Setup(x => x.GetProfileDirectory())
            .Returns("/Users/testuser");
        }
        break;
    }

    var instanceOptions = new InstanceOptions { InstanceId = instanceId };
    var optionsWrapper = new OptionsMonitorWrapper<InstanceOptions>(instanceOptions);

    _elevationChecker = new Mock<IElevationChecker>();
    _elevationChecker.Setup(x => x.IsElevated()).Returns(isElevated);
    
    var directorySeparator = platform == SystemPlatform.Windows ? '\\' : '/';
    _fileSystem = new FakeFileSystem(directorySeparator);
    
    _pathProvider = new FileSystemPathProvider(
      _systemEnvironment.Object,
      _elevationChecker.Object,
      _fileSystem,
      optionsWrapper);
  }
}

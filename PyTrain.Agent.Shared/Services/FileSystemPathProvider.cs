using PyTrain.Agent.Shared.Constants;
using PyTrain.Agent.Shared.Options;
using PyTrain.Libraries.Shared.Constants;
using PyTrain.Libraries.Shared.Services.FileSystem;
using Microsoft.Extensions.Options;

namespace PyTrain.Agent.Shared.Services;

public interface IFileSystemPathProvider
{
  string GetAgentAppSettingsPath();
  string GetAgentLogFilePath();
  string GetAgentLogsDirectoryPath();
  /// <summary>
  /// Returns the path where the bundle hash file is stored.
  /// This file contains the SHA-256 hash of the currently installed bundle and is used by the updater
  /// to validate whether the installation is up to date with the server's latest bundle.
  /// </summary>
  string GetBundleHashFilePath();
  /// <summary>
  /// Returns the directory where the agent executable resides, which is also the bundle root after installation.
  /// Currently returns the startup directory; used for centralizing assumptions about where the bundle is installed.
  /// </summary>
  string GetBundleRootDirectory();
  string GetDesktopClientDirectory();
  string GetDesktopExecutablePath();
  string GetInstallerLogFilePath();
  string GetInstallerLogsDirectoryPath();
  string GetUnixDesktopClientLogsDirectory(string username);
  string GetUnixDesktopClientLogsDirectoryForRoot();
  string GetWindowsDesktopClientLogsDirectory();
}
public class FileSystemPathProvider(
  ISystemEnvironment systemEnvironment,
  IElevationChecker elevationChecker,
  IFileSystem fileSystem,
  IOptionsMonitor<InstanceOptions> instanceOptions) : IFileSystemPathProvider
{
  private readonly IElevationChecker _elevationChecker = elevationChecker;
  private readonly IFileSystem _fileSystem = fileSystem;
  private readonly IOptionsMonitor<InstanceOptions> _instanceOptions = instanceOptions;
  private readonly ISystemEnvironment _systemEnvironment = systemEnvironment;

  public string GetAgentAppSettingsPath()
  {
    var dir = GetSettingsDirectory();
    return _fileSystem.JoinPaths(GetPathSeparator(), dir, "appsettings.json");
  }

  public string GetAgentLogFilePath()
  {
    return _fileSystem.JoinPaths(GetPathSeparator(), GetAgentLogsDirectoryPath(), "LogFile.log");
  }

  public string GetAgentLogsDirectoryPath()
  {
    if (_systemEnvironment.IsWindows())
    {
      var logsDir = _fileSystem.JoinPaths(GetPathSeparator(),
        _systemEnvironment.GetCommonApplicationDataDirectory(),
        "PyTrain");

      logsDir = AppendSubDirectories(logsDir);
      return _fileSystem.JoinPaths(GetPathSeparator(), logsDir, "Logs", "PyTrain.Agent");
    }

    if (_systemEnvironment.IsLinux() || _systemEnvironment.IsMacOS())
    {
      var isElevated = _elevationChecker.IsElevated();
      var rootDir = isElevated
        ? "/var/log/pytrain"
        : _fileSystem.JoinPaths(GetPathSeparator(), _systemEnvironment.GetProfileDirectory(), ".pytrain");

      rootDir = AppendSubDirectories(rootDir);
      var logsDir = isElevated ? rootDir : _fileSystem.JoinPaths(GetPathSeparator(), rootDir, "logs");
      return _fileSystem.JoinPaths(GetPathSeparator(), logsDir, "PyTrain.Agent");
    }

    throw new PlatformNotSupportedException();
  }

  public string GetBundleHashFilePath()
  {
    var settingsDirectory = GetSettingsDirectory();
    return _fileSystem.JoinPaths(GetPathSeparator(), settingsDirectory, ".pytrain-bundle.sha256");
  }

  public string GetBundleRootDirectory()
  {
    if (_systemEnvironment.IsMacOS())
    {
      return GetMacInstalledAppPath();
    }

    return _systemEnvironment.StartupDirectory;
  }

  public string GetDesktopClientDirectory()
  {
    if (_systemEnvironment.IsMacOS())
    {
      return GetMacInstalledAppPath();
    }

    var startupDir = _systemEnvironment.StartupDirectory;
    return Path.Combine(startupDir, "DesktopClient");
  }

  public string GetDesktopExecutablePath()
  {
    var desktopDir = GetDesktopClientDirectory();

    return _systemEnvironment.Platform switch
    {
      SystemPlatform.MacOs => _fileSystem.JoinPaths('/', desktopDir, PathConstants.MacDesktopExecutableRelativePath),
      _ => Path.Combine(desktopDir, AppConstants.DesktopClientFileName)
    };
  }

  public string GetInstallerLogFilePath()
  {
    return _fileSystem.JoinPaths(GetPathSeparator(), GetInstallerLogsDirectoryPath(), "LogFile.log");
  }

  public string GetInstallerLogsDirectoryPath()
  {
    if (_systemEnvironment.IsWindows())
    {
      var logsDir = _fileSystem.JoinPaths(GetPathSeparator(),
        _systemEnvironment.GetCommonApplicationDataDirectory(),
        "PyTrain");

      logsDir = AppendSubDirectories(logsDir);
      return _fileSystem.JoinPaths(GetPathSeparator(), logsDir, "Logs", "PyTrain.Agent.Installer");
    }

    if (_systemEnvironment.IsLinux() || _systemEnvironment.IsMacOS())
    {
      var isElevated = _elevationChecker.IsElevated();
      var rootDir = isElevated
        ? "/var/log/pytrain"
        : _fileSystem.JoinPaths(GetPathSeparator(), _systemEnvironment.GetProfileDirectory(), ".pytrain");

      rootDir = AppendSubDirectories(rootDir);
      var logsDir = isElevated ? rootDir : _fileSystem.JoinPaths(GetPathSeparator(), rootDir, "logs");
      return _fileSystem.JoinPaths(GetPathSeparator(), logsDir, "PyTrain.Agent.Installer");
    }

    throw new PlatformNotSupportedException();
  }

  public string GetUnixDesktopClientLogsDirectory(string username)
  {
    if (!_systemEnvironment.IsLinux() && !_systemEnvironment.IsMacOS())
    {
      throw new PlatformNotSupportedException();
    }

    if (string.IsNullOrWhiteSpace(username))
    {
      throw new ArgumentException("Username must be provided for non-root log directory.", nameof(username));
    }

    var instanceId = GetEffectiveInstanceId();
    var homeRoot = _systemEnvironment.IsMacOS() ? "/Users" : "/home";

    return _fileSystem.JoinPaths(GetPathSeparator(), homeRoot, username, ".pytrain", instanceId, "logs", "PyTrain.DesktopClient");
  }

  public string GetUnixDesktopClientLogsDirectoryForRoot()
  {
    if (!_systemEnvironment.IsLinux() && !_systemEnvironment.IsMacOS())
    {
      throw new PlatformNotSupportedException();
    }

    var instanceId = GetEffectiveInstanceId();
    var logsDir = "/var/log/pytrain";

    logsDir = _fileSystem.JoinPaths(GetPathSeparator(), logsDir, instanceId);

    return _fileSystem.JoinPaths(GetPathSeparator(), logsDir, "PyTrain.DesktopClient");
  }

  public string GetWindowsDesktopClientLogsDirectory()
  {
    if (!_systemEnvironment.IsWindows())
    {
      throw new PlatformNotSupportedException();
    }

    var isDebug = _systemEnvironment.IsDebug;

    var logsDir = _fileSystem.JoinPaths(GetPathSeparator(),
         _systemEnvironment.GetCommonApplicationDataDirectory(),
         "PyTrain");

    if (isDebug)
    {
      logsDir = _fileSystem.JoinPaths(GetPathSeparator(), logsDir, "Debug");
    }

    logsDir = _fileSystem.JoinPaths(GetPathSeparator(), logsDir, GetEffectiveInstanceId());

    return _fileSystem.JoinPaths(GetPathSeparator(), logsDir, "Logs", "PyTrain.DesktopClient");

  }

  private string AppendSubDirectories(string rootDir)
  {
    var instanceId = GetEffectiveInstanceId();

    if (_systemEnvironment.IsWindows())
    {
      if (_systemEnvironment.IsDebug)
      {
        rootDir = _fileSystem.JoinPaths(GetPathSeparator(), rootDir, "Debug");
      }

      rootDir = _fileSystem.JoinPaths(GetPathSeparator(), rootDir, instanceId);

      _ = _fileSystem.CreateDirectory(rootDir).FullName;
      return rootDir;
    }

    // ReSharper disable once InvertIf
    if (_systemEnvironment.IsLinux() || _systemEnvironment.IsMacOS())
    {
      rootDir = _fileSystem.JoinPaths(GetPathSeparator(), rootDir, instanceId);

      _ = _fileSystem.CreateDirectory(rootDir).FullName;
      return rootDir;
    }

    throw new PlatformNotSupportedException();
  }

  private string GetEffectiveInstanceId()
  {
    return string.IsNullOrWhiteSpace(_instanceOptions.CurrentValue.InstanceId)
      ? AppConstants.DefaultInstallDirectoryName
      : _instanceOptions.CurrentValue.InstanceId!;
  }

  private string GetMacInstalledAppPath()
  {
    return PathConstants.GetMacInstalledAppPath(_instanceOptions.CurrentValue.InstanceId);
  }

  private char GetPathSeparator()
  {
    return _systemEnvironment.IsWindows() ? '\\' : '/';
  }

  private string GetSettingsDirectory()
  {
    if (_systemEnvironment.IsWindows())
    {
      var rootDir = _fileSystem.JoinPaths(
        GetPathSeparator(),
        _systemEnvironment.GetCommonApplicationDataDirectory(),
        "PyTrain");

      return AppendSubDirectories(rootDir);
    }

    // ReSharper disable once InvertIf
    if (_systemEnvironment.IsLinux() || _systemEnvironment.IsMacOS())
    {
      var rootDir = _elevationChecker.IsElevated()
        ? "/etc/pytrain"
        : _fileSystem.JoinPaths(GetPathSeparator(), _systemEnvironment.GetProfileDirectory(), ".pytrain");

      return AppendSubDirectories(rootDir);
    }

    throw new PlatformNotSupportedException();
  }

}

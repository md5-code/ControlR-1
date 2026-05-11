using PyTrain.Libraries.Shared.Constants;
using PyTrain.Libraries.NativeInterop.Unix;

namespace PyTrain.DesktopClient.Mac;

public static class PathConstants
{
  public static string GetLogsPath(string? instanceId)
  {
    var isRoot = Libc.Geteuid() == 0;
    var rootDir = isRoot
       ? "/var/log/pytrain"
       : $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.pytrain";

    rootDir = AppendInstanceId(rootDir, instanceId);
    var logsDir = isRoot ? rootDir : Path.Combine(rootDir, "logs");
    return Path.Combine(logsDir, "PyTrain.DesktopClient", "LogFile.log");
  }

  private static string AppendInstanceId(string rootDir, string? instanceId)
  {
    return Path.Combine(rootDir, GetEffectiveInstanceId(instanceId));
  }

  private static string GetEffectiveInstanceId(string? instanceId)
  {
    return string.IsNullOrWhiteSpace(instanceId)
      ? AppConstants.DefaultInstallDirectoryName
      : instanceId;
  }

}
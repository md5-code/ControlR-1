namespace PyTrain.Libraries.Ipc;
public static class IpcPipeNames
{
  public static string GetPipeName(string? instanceId)
  {
    if (OperatingSystem.IsWindows())
    {
      return GetWindowsPipeName(instanceId);
    }

    if (OperatingSystem.IsMacOS() || OperatingSystem.IsLinux())
    {
      return GetUnixPipeName(instanceId);
    }

    throw new PlatformNotSupportedException();
  }

  public static string GetUnixPipeName(string? instanceId)
  {
#if DEBUG
    var pipeName = "/tmp/pytrain-ipc-server-debug";
#else
    var pipeName = "/tmp/pytrain-ipc-server";
#endif
    if (string.IsNullOrWhiteSpace(instanceId))
    {
      return pipeName;
    }
    return $"{pipeName}-{instanceId.Replace(".", "-")}";
  }

  public static string GetWindowsPipeName(string? instanceId)
  {
#if DEBUG
    var pipeName = "pytrain-ipc-server-debug";
#else
    var pipeName = "pytrain-ipc-server";
#endif
    if (string.IsNullOrWhiteSpace(instanceId))
    {
      return pipeName;
    }
    return $"{pipeName}-{instanceId.Replace(".", "-")}";
  }
}

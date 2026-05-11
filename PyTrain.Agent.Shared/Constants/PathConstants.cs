namespace PyTrain.Agent.Shared.Constants;

public static class PathConstants
{
  public const string MacApplicationsDirectory = "/Applications";
  public static string MacDesktopExecutableRelativePath => "Contents/MacOS/PyTrain.DesktopClient";

  public static string GetMacAppBundleName(string? instanceId)
  {
    return string.IsNullOrWhiteSpace(instanceId)
      ? "PyTrain.app"
      : $"PyTrain.{instanceId}.app";
  }

  public static string GetMacInstalledAppPath(string? instanceId)
  {
    var appBundleName = GetMacAppBundleName(instanceId);

    return $"{MacApplicationsDirectory}/{appBundleName}";
  }
}
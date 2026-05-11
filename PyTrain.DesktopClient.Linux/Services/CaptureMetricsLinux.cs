using PyTrain.DesktopClient.Common.ServiceInterfaces;

namespace PyTrain.DesktopClient.Linux.Services;

public class CaptureMetricsLinux : ICaptureMetrics
{
  public Dictionary<string, string> GetExtraMetricsData()
  {
    return [];
  }
}

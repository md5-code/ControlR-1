using PyTrain.DesktopClient.Common.ServiceInterfaces;

namespace PyTrain.DesktopClient.Mac.Services;

public class CaptureMetricsMac : ICaptureMetrics
{
  public Dictionary<string, string> GetExtraMetricsData()
  {
    return [];
  }
}

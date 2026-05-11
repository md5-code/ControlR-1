using PyTrain.DesktopClient.Common.ServiceInterfaces;
using PyTrain.Libraries.NativeInterop.Windows;
using PyTrain.Libraries.Shared.Services;
using Microsoft.Extensions.DependencyInjection;

namespace PyTrain.DesktopClient.Windows.Services;

internal sealed class CaptureMetricsWindows(IServiceProvider serviceProvider) : ICaptureMetrics
{
  private readonly ISystemEnvironment _systemEnvironment = serviceProvider.GetRequiredService<ISystemEnvironment>();
  private readonly IWin32Interop _win32Interop = serviceProvider.GetRequiredService<IWin32Interop>();

  public Dictionary<string, string> GetExtraMetricsData()
  {
    var extraData = new Dictionary<string, string>();

    _ = _win32Interop.GetCurrentThreadDesktopName(out var threadDesktopName);
    _ = _win32Interop.GetInputDesktopName(out var inputDesktopName);

    extraData.Add("Thread ID", $"{_systemEnvironment.CurrentThreadId}");
    extraData.Add("Thread Desktop Name", $"{threadDesktopName}");
    extraData.Add("Input Desktop Name", $"{inputDesktopName}");

    return extraData;
  }
}

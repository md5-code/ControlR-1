using PyTrain.Agent.Shared.Interfaces;
using PyTrain.Libraries.NativeInterop.Unix;

namespace PyTrain.Agent.Shared.Services.Linux;

public class ElevationCheckerLinux : IElevationChecker
{
  public static IElevationChecker Instance { get; } = new ElevationCheckerLinux();

  public bool IsElevated()
  {
    return Libc.Geteuid() == 0;
  }
}

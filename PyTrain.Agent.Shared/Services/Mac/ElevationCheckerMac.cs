using PyTrain.Agent.Shared.Interfaces;
using PyTrain.Libraries.NativeInterop.Unix;

namespace PyTrain.Agent.Shared.Services.Mac;

public class ElevationCheckerMac : IElevationChecker
{
  public static IElevationChecker Instance { get; } = new ElevationCheckerMac();

  public bool IsElevated()
  {
    return Libc.Geteuid() == 0;
  }
}

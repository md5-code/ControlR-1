using PyTrain.Agent.Common.Interfaces;

namespace PyTrain.Agent.Common.Services;

public class DesktopClientFileVerifierMac : IDesktopClientFileVerifier
{
  public Result VerifyFile(string executablePath)
  {
    return Result.Ok();
  }
}

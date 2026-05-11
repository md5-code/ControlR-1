using System.Runtime.Versioning;
using PyTrain.Agent.Common.Interfaces;
using PyTrain.Libraries.Ipc;
using PyTrain.Libraries.NativeInterop.Windows;

namespace PyTrain.Agent.Common.Services.Windows;

[SupportedOSPlatform("windows")]
internal class IpcClientCredentialsProviderWindows : IIpcClientCredentialsProvider
{
  public Result<IpcClientCredentials> GetClientCredentials(IIpcServer server)
  {
    if (!server.TryGetServerHandle(out var handle))
    {
      return Result.Fail<IpcClientCredentials>("Failed to get server handle from IPC connection.");
    }

    var nativeResult = PipeClientInfo.GetClientCredentials(handle);
    if (!nativeResult.IsSuccess)
    {
      return Result.Fail<IpcClientCredentials>(nativeResult.Reason);
    }

    // Convert native ClientCredentials to common ClientCredentials
    return Result.Ok(new IpcClientCredentials(
      nativeResult.Value.ProcessId,
      nativeResult.Value.ExecutablePath));
  }
}

using PyTrain.Libraries.Ipc;

namespace PyTrain.Agent.Common.Interfaces;

public interface IIpcClientCredentialsProvider
{
  Result<IpcClientCredentials> GetClientCredentials(IIpcServer server);
}

using System.Diagnostics.CodeAnalysis;
using PyTrain.Libraries.Ipc;

namespace PyTrain.DesktopClient.Common.ServiceInterfaces;

public interface IIpcClientAccessor
{
  bool TryGetClient([NotNullWhen(true)] out IIpcClient? connection);
}

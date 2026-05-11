using PyTrain.Libraries.Api.Contracts.Dtos.IpcDtos;
using Microsoft.Extensions.Hosting;

namespace PyTrain.DesktopClient.Common.ServiceInterfaces;

public interface IRemoteControlHostBuilderFactory
{
  HostApplicationBuilder CreateHostBuilder(RemoteControlRequestIpcDto requestDto);
}

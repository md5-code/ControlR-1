using PyTrain.DesktopClient.Common;
using PyTrain.DesktopClient.Common.Options;
using PyTrain.DesktopClient.Common.ServiceInterfaces;
using PyTrain.DesktopClient.Common.ServiceInterfaces.Toaster;
using PyTrain.DesktopClient.Mac;
using PyTrain.Libraries.Api.Contracts.Dtos.IpcDtos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace PyTrain.DesktopClient.Mac.Services;

public class MacRemoteControlHostBuilderFactory(
  IOptionsMonitor<DesktopClientOptions> desktopClientOptions,
  IUserInteractionService userInteractionService,
  IIpcClientAccessor ipcClientAccessor,
  IToaster toaster,
  IUiThread uiThread) : IRemoteControlHostBuilderFactory
{
  private readonly IOptionsMonitor<DesktopClientOptions> _desktopClientOptions = desktopClientOptions;
  private readonly IIpcClientAccessor _ipcClientAccessor = ipcClientAccessor;
  private readonly IToaster _toaster = toaster;
  private readonly IUiThread _uiThread = uiThread;
  private readonly IUserInteractionService _userInteractionService = userInteractionService;

  public HostApplicationBuilder CreateHostBuilder(RemoteControlRequestIpcDto requestDto)
  {
    var builder = Host.CreateApplicationBuilder();

    builder.AddCommonRemoteControlServices(
      appBuilder =>
      {
        appBuilder.Services
          .AddSingleton(_toaster)
          .AddSingleton(_uiThread)
          .AddSingleton(_userInteractionService)
          .AddSingleton(_ipcClientAccessor);
      },
      options =>
      {
        options.SessionId = requestDto.SessionId;
        options.NotifyUser = requestDto.NotifyUserOnSessionStart;
        options.RequireConsent = requestDto.RequireConsent;
        options.ViewerName = requestDto.ViewerName;
        options.ViewerConnectionId = requestDto.ViewerConnectionId;
        options.WebSocketUri = requestDto.WebsocketUri;
      },
      options =>
      {
        options.InstanceId = _desktopClientOptions.CurrentValue.InstanceId;
      });

    builder.AddRemoteControlPlatformServices();
    return builder;
  }
}

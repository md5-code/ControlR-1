using System.Threading.Channels;
using PyTrain.Libraries.Api.Contracts.Dtos;
using PyTrain.Libraries.Api.Contracts.Dtos.Devices;
using PyTrain.Libraries.Api.Contracts.Dtos.HubDtos;
using PyTrain.Libraries.Api.Contracts.Dtos.HubDtos.PwshCommandCompletions;
using PyTrain.Libraries.Api.Contracts.Dtos.RemoteControlDtos;
using PyTrain.Libraries.Api.Contracts.Enums;

namespace PyTrain.Libraries.Api.Contracts.Hubs;

public interface IViewerHub
{
  Task<HubResult> CloseChatSession(Guid deviceId, Guid sessionId, int targetProcessId);
  Task CloseTerminalSession(Guid deviceId, Guid terminalSessionId);

  Task<HubResult> CreateTerminalSession(
    Guid deviceId,
    Guid terminalSessionId);

  Task<DesktopSession[]> GetActiveDesktopSessions(Guid deviceId);
  Task<HubResult<PwshCompletionsResponseDto>> GetPwshCompletions(PwshCompletionsRequestDto request);
  Task<HubResult> InvokeCtrlAltDel(Guid deviceId, int targetDesktopProcessId, DesktopSessionType desktopSessionType);
  Task RefreshDeviceInfo(Guid deviceId);
  Task<HubResult> RequestRemoteControlPermission(Guid deviceId, int targetProcessId);

  Task<HubResult> RequestRemoteControlSession(Guid deviceId, RemoteControlSessionRequestDto sessionRequestDto);
  Task<HubResult> RequestVncSession(Guid deviceId, VncSessionRequestDto sessionRequestDto);
  Task SendAgentUpdateTrigger(Guid deviceId);
  Task<HubResult> SendChatMessage(Guid deviceId, ChatMessageHubDto dto);
  Task SendDtoToAgent(Guid deviceId, DtoWrapper wrapper);
  Task SendDtoToUserGroups(DtoWrapper wrapper);
  Task SendPowerStateChange(Guid deviceId, PowerStateChangeType changeType);
  Task<HubResult> SendTerminalInput(Guid deviceId, TerminalInputDto dto);
  Task SendWakeDevice(Guid deviceId, string[] macAddresses);
  Task<HubResult> TestVncConnection(Guid guid, int port);
  Task UninstallAgent(Guid deviceId, string reason);
  Task<HubResult> UploadFile(FileUploadMetadata metadata, ChannelReader<byte[]> fileStream);
}
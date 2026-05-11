using PyTrain.Libraries.Api.Contracts.Dtos.IpcDtos;
using PyTrain.Libraries.Api.Contracts.Dtos.HubDtos;
using PyTrain.Libraries.Shared.Primitives;
using StreamJsonRpc;
using PolyType;

namespace PyTrain.Libraries.Ipc.Interfaces;

[JsonRpcContract]
[GenerateShape(IncludeMethods = MethodShapeFlags.PublicInstance)]
public partial interface IDesktopClientRpcService
{
    Task<CheckOsPermissionsResponseIpcDto> CheckOsPermissions(CheckOsPermissionsIpcDto dto);
    Task CloseChatSession(CloseChatSessionIpcDto dto);
    Task<DesktopPreviewResponseIpcDto> GetDesktopPreview(DesktopPreviewRequestIpcDto dto);
    Task InvokeCtrlAltDel(InvokeCtrlAltDelRequestDto dto);
    Task ReceiveChatMessage(ChatMessageIpcDto dto);
    Task<Result> ReceiveRemoteControlRequest(RemoteControlRequestIpcDto dto);
    Task<CheckOsPermissionsResponseIpcDto> RequestRemoteControlPermission(RequestRemoteControlPermissionIpcDto dto);
    Task ShutdownDesktopClient(ShutdownCommandDto dto);
}

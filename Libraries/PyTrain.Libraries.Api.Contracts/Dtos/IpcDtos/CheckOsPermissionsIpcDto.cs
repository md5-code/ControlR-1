namespace PyTrain.Libraries.Api.Contracts.Dtos.IpcDtos;

public enum DesktopClientPermissionScope
{
  RemoteControl = 0,
  DesktopPreview = 1
}

/// <summary>
/// IPC request to check OS-level desktop client permission status.
/// </summary>
[MessagePackObject(keyAsPropertyName: true)]
public record CheckOsPermissionsIpcDto(
  int TargetProcessId,
  DesktopClientPermissionScope Scope = DesktopClientPermissionScope.RemoteControl);

namespace PyTrain.Libraries.Api.Contracts.Dtos.IpcDtos;

[MessagePackObject(keyAsPropertyName: true)]
public record CloseChatSessionIpcDto(
  Guid SessionId,
  int TargetProcessId);

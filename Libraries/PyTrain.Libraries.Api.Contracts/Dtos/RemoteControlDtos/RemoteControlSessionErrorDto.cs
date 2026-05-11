using MessagePack;

namespace PyTrain.Libraries.Api.Contracts.Dtos.RemoteControlDtos;

[MessagePackObject(keyAsPropertyName: true)]
public record RemoteControlSessionErrorDto(
  string Message,
  bool IsFatal);

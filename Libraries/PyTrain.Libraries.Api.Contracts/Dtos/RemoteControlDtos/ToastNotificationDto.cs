using PyTrain.Libraries.Api.Contracts.Enums;
using MessagePack;

namespace PyTrain.Libraries.Api.Contracts.Dtos.RemoteControlDtos;

[MessagePackObject(keyAsPropertyName: true)]
public record ToastNotificationDto(
  string Message,
  MessageSeverity Severity);

using PyTrain.Libraries.Api.Contracts.Dtos.HubDtos;

namespace PyTrain.Libraries.Api.Contracts.Dtos.RemoteControlDtos;

[MessagePackObject(keyAsPropertyName: true)]
public record DisplayDataDto(DisplayDto[] Displays);
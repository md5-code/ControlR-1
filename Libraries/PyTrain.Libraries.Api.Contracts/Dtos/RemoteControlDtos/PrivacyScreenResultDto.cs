using MessagePack;

namespace PyTrain.Libraries.Api.Contracts.Dtos.RemoteControlDtos;

[MessagePackObject(keyAsPropertyName: true)]
public record PrivacyScreenResultDto(bool IsSuccess, bool FinalState);

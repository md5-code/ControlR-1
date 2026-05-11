using PyTrain.Libraries.Api.Contracts.Enums;

namespace PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;

public record TagCreateRequestDto(string Name, TagType Type);
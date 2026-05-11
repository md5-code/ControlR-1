namespace PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;

public record UpdateDeviceAliasRequestDto(Guid DeviceId, string? Alias);

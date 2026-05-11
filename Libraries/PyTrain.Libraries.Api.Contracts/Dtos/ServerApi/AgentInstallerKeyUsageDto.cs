namespace PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;

public record AgentInstallerKeyUsageDto(
    Guid Id,
    Guid DeviceId,
    DateTimeOffset Timestamp,
    string? RemoteIpAddress);

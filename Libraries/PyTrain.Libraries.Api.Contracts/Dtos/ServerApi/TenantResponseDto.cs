namespace PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;

public record TenantResponseDto(
  Guid Id,
  string Name,
  int UserCount,
  int DeviceCount,
  DateTimeOffset CreatedAt);

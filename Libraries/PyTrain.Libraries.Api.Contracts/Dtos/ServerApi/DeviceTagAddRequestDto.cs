namespace PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;

public record DeviceTagAddRequestDto(
  Guid DeviceId,
  Guid TagId);
namespace PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;

public record UserTagAddRequestDto(
  Guid UserId,
  Guid TagId);
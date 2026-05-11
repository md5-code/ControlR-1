namespace PyTrain.Libraries.Api.Contracts.Dtos.HubDtos;

public record DirectoryContentsStreamRequestHubDto(
  Guid StreamId,
  Guid DeviceId,
  string DirectoryPath);

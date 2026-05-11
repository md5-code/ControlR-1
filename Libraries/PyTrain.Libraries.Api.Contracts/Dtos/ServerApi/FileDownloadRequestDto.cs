namespace PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;

public record FileDownloadRequestDto(
  Guid DeviceId,
  string FilePath,
  bool IsDirectory);

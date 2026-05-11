namespace PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;

public record DownloadArchiveRequestDto(
  string ArchiveFileName,
  string[] TargetPaths);
using PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;

namespace PyTrain.Agent.Common.Services.FileManager;

public record DirectoryContentsResult(
  FileSystemEntryDto[] Entries,
  bool DirectoryExists);

namespace PyTrain.Libraries.Api.Contracts.Dtos.IpcDtos;

[MessagePackObject(keyAsPropertyName: true)]
public record ShutdownCommandDto(string Reason);
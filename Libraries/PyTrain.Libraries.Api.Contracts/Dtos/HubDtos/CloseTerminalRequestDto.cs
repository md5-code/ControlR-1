namespace PyTrain.Libraries.Api.Contracts.Dtos.HubDtos;

[MessagePackObject]
public record CloseTerminalRequestDto([property: Key(0)] Guid TerminalId);
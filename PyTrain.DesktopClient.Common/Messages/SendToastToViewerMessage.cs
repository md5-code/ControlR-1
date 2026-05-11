using PyTrain.Libraries.Api.Contracts.Enums;

namespace PyTrain.DesktopClient.Common.Messages;

public sealed record SendToastToViewerMessage(string Message, MessageSeverity Severity);
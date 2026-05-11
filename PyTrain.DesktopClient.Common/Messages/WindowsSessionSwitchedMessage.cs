namespace PyTrain.DesktopClient.Common.Messages;

public record WindowsSessionSwitchedMessage(SessionSwitchReasonEx Reason, int SessionId);
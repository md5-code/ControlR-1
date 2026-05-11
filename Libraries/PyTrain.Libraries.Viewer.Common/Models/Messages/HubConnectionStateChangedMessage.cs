using Microsoft.AspNetCore.SignalR.Client;

namespace PyTrain.Libraries.Viewer.Common.Models.Messages;

public record HubConnectionStateChangedMessage(HubConnectionState NewState);

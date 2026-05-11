# PyTrain.Libraries.Viewer.Common

A shared library providing common models, services, state management, and enums for PyTrain remote viewer applications.

## Features

### Enums

- **ControlMode**: Defines the control mode for remote control sessions (e.g., None, View, Control)
- **SignalingState**: Represents the signaling state for WebRTC connections
- **ViewMode**: Defines how the remote screen is displayed (e.g., Fit, Original, Scale)
- **WindowState**: Represents the window state (e.g., Normal, Minimized, Maximized, Fullscreen)

### Models

- **RemoteControlSession**: Model representing a remote control session with all related properties
- **Messages**:
  - `ChatMessage` - Chat message model
  - `DesktopChangedMessage` - Message for desktop/screen changes
  - `HubConnectionStateChangedMessage` - Message for connection state changes

### Options

- **PyTrainViewerOptions**: Configuration options for the viewer

### Services

- **ViewerHubClient**: Implementation of `IViewerHubClient` for handling hub communications
- **ViewerHubConnector**: Helper for establishing and managing hub connections

### State Management

- **ChatState**: State management for chat functionality
- **DeviceState**: State management for device information
- **MetricsState**: State management for performance metrics
- **RemoteControlState**: State management for remote control sessions
- **TerminalState**: State management for terminal sessions

### Streaming

- **ViewerRemoteControlStream**: Stream handling for remote control video data

## Usage

### Basic Usage

```csharp
using PyTrain.Libraries.Viewer.Common;
using PyTrain.Libraries.Viewer.Common.Services;
using PyTrain.Libraries.Viewer.Common.State;
using PyTrain.Libraries.Viewer.Common.Enums;
using PyTrain.Libraries.Messenger.Extensions;
using Microsoft.Extensions.Logging;

// Create viewer options
var options = new PyTrainViewerOptions
{
    ServerUri = "https://pytrain.example.com",
    AccessToken = "your-token"
};

// Use ViewerHubConnector to connect
var connector = new ViewerHubConnector(messenger, loggerFactory, options);
await connector.ConnectAsync();

// Access the hub client
var hubClient = connector.GetHubClient<IViewerHubClient>();
```

### State Management

```csharp
// Use state classes for reactive UI
var remoteControlState = new RemoteControlState(messenger, logger);

// Subscribe to state changes
remoteControlState.PropertyChanged += (sender, args) =>
{
    if (args.PropertyName == nameof(RemoteControlState.IsConnected))
    {
        Console.WriteLine($"Connected: {remoteControlState.IsConnected}");
    }
};
```

### Remote Control

```csharp
// Create a remote control session
var session = new RemoteControlSession
{
    SessionId = Guid.NewGuid(),
    DeviceId = deviceId,
    ControlMode = ControlMode.Control,
    ViewMode = ViewMode.Fit
};

// Start remote control
await viewerHub.StartRemoteControl(session.DeviceId, session.ControlMode);
```

## Architecture

The library is organized as follows:

- `Enums/` - Enumeration definitions
- `Models/` - Data models
  - `Messages/` - Message models
- `Options/` - Configuration options
- `Services/` - Service implementations
- `State/` - State management classes
- `ViewerRemoteControlStream.cs` - Stream handling

## Integration with Messenger

The library uses `Bitbound.SimpleMessenger` for messaging:

```csharp
// Register for device updates
messenger.Register<DtoReceivedMessage<DeviceResponseDto>>(this, async (recipient, message) =>
{
    var device = message.Data;
    // Handle device update
    await Task.CompletedTask;
});

// Register for connection state changes
messenger.Register<HubConnectionStateChangedMessage>(this, async (recipient, message) =>
{
    Console.WriteLine($"Connection state: {message.State}");
    await Task.CompletedTask;
});
```

## Dependencies

This library depends on:

- PyTrain.Libraries.Api.Contracts
- PyTrain.Libraries.Messenger.Extensions
- PyTrain.Libraries.Shared
- Microsoft.Extensions.Logging
- Bitbound.SimpleMessenger

## Use Cases

- Desktop viewer applications (Windows, Linux, macOS)
- Web-based viewer components
- Mobile viewer implementations
- Any application that needs to connect to PyTrain for remote control functionality

# PyTrain.Viewer.Avalonia

`PyTrain.Viewer.Avalonia` provides the `PyTrainViewer` control for embedding a PyTrain remote viewer inside an Avalonia UI application.

Use it when you want to host a live PyTrain session inside one of your existing views (not necessarily your main window).

## Install

```bash
dotnet add package PyTrain.Viewer.Avalonia
```

## Requirements

Your view model must expose a `PyTrainViewerOptions` instance containing:

- `BaseUrl` (`Uri`) - Base URL of your PyTrain server.
- `DeviceId` (`Guid`) - Device to connect to.
- `PersonalAccessToken` (`string`) - PAT for the connecting user.

How those values are resolved is up to your application architecture.

## Core APIs

`PyTrainViewer` exposes these primary bindable/public members:

- `Options` (`PyTrainViewerOptions?`) - connection settings for the viewer instance.
- `Page` (`ViewerPage`) - declaratively selects the active page.
- `InstanceId` (`Guid`) - unique ID assigned to the viewer instance after construction.
- `GetInstanceInfo()` - returns the public-facing `ViewerInstanceInfo` for this viewer instance.
- `GetRequiredService<T>()` - resolves a required service from this viewer instance.
- `GetService<T>()` - resolves an optional service from this viewer instance.

Available `ViewerPage` values:

- `None`
- `RemoteControl`
- `FileSystem`
- `Terminal`

## Usage Example

In this example, `PyTrainViewer` is hosted in `ParentView.axaml`, with options provided by `ParentViewModel`.

### ParentView.axaml

```xml
<UserControl xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:vm="using:YourApp.ViewModels"
             xmlns:ctrlr="using:PyTrain.Viewer.Avalonia"
             xmlns:nav="using:PyTrain.Viewer.Avalonia.Services.Navigation"
             x:Class="YourApp.Views.ParentView"
             x:DataType="vm:ParentViewModel">

	<ctrlr:PyTrainViewer Options="{Binding ViewerOptions}"
	                     Page="{Binding CurrentPage}" />
</UserControl>
```

### ParentViewModel.cs

```csharp
using PyTrain.Libraries.Viewer.Common.Options;
using PyTrain.Viewer.Avalonia.Services.Navigation;
using Microsoft.Extensions.Options;

namespace YourApp.ViewModels;

public class ParentViewModel
{
  public ParentViewModel(IOptions<PyTrainViewerOptions> viewerOptions)
  {
    ViewerOptions = viewerOptions.Value;
  }

  public ViewerPage CurrentPage { get; set; } = ViewerPage.RemoteControl;
  public PyTrainViewerOptions ViewerOptions { get; }
}
```

## Declarative Navigation

Set the `Page` property to control which page the embedded viewer should display.

```xml
<ctrlr:PyTrainViewer Options="{Binding ViewerOptions}"
                     Page="{Binding CurrentPage}" />
```

```csharp
CurrentPage = ViewerPage.FileSystem;
```

If the viewer is not fully connected yet, the requested page is stored and applied once navigation becomes available.

## Imperative Navigation

For imperative page changes, resolve `INavigator` from the viewer instance and call `NavigateTo`.

```csharp
using PyTrain.Viewer.Avalonia.Services.Navigation;

var navigator = pytrainViewer.GetInstanceInfo().GetNavigator();
var result = await navigator.NavigateTo(ViewerPage.Terminal);

if (!result.IsSuccess)
{
  // Handle result.Reason.
}
```

You can also get `INavigator` through `ViewerRegistry` or `ViewerInstanceInfo` if you are operating from outside the control tree.

## ViewerRegistry

The library exposes a global `ViewerRegistry` helper (in `PyTrain.Viewer.Avalonia.Services`) that stores information about the `PyTrainViewer` and `IServiceProvider` instances that are currently active. This allows you to interact with the viewers and their services from anywhere in your application, as long as you have the viewer's instance ID (which is a `Guid`).

Instances are automatically registered when a `PyTrainViewer` is initialized and unregistered/disposed when the `PyTrainViewer` instance is detached from the visual tree, so you don't need to manage the lifecycle manually.

Important APIs:

- `ViewerRegistry.Register(Guid instanceId, PyTrainViewer viewer, IServiceProvider serviceProvider)` — registers a viewer instance (the control registers itself automatically).
- `ViewerRegistry.Unregister(Guid instanceId)` — removes a registered instance (the control unregisters itself on disposal).
- `ViewerRegistry.GetRequiredService<T>(Guid instanceId)` — resolves a required service from a specific viewer's scope and throws if not found.
- `ViewerRegistry.GetService<T>(Guid instanceId)` — attempts to resolve a service and returns null if not found.
- `ViewerRegistry.GetService(Guid instanceId, Type serviceType)` — non-generic service resolution.
- `ViewerRegistry.GetAllInstanceIds()` — returns all currently registered viewer instance IDs.
- `ViewerRegistry.TryGetInstance(Guid instanceId, out ViewerInstanceInfo? viewerInstanceInfo)` — attempts to get a registered viewer instance plus its service provider.

`ViewerInstanceInfo` exposes:

- `InstanceId`
- `Viewer`
- `ServiceProvider`
- `GetHubConnection()`
- `GetPyTrainApi()`
- `GetNavigator()`

Example usage:

```csharp
using PyTrain.Viewer.Avalonia.Services;
using PyTrain.Viewer.Avalonia.Services.Navigation;

if (ViewerRegistry.TryGetInstance(viewerId, out var instance))
{
  var navigator = instance.GetNavigator();
  await navigator.NavigateTo(ViewerPage.FileSystem);

  var api = instance.GetPyTrainApi();
  var hubConnection = instance.GetHubConnection();
}
```

You can also resolve arbitrary services directly:

```csharp
var remoteControlStream = ViewerRegistry.GetRequiredService<IViewerRemoteControlStream>(viewerId);
```

## Notes

- Keep PATs out of source control.
- The `PyTrainViewer` initializes and connects to the server once it becomes visible.
- Options will be validated automatically before connecting.
  - If any options are missing or invalid, the control will render an error message instead.
- Viewer instances register themselves with `ViewerRegistry` after their internal service provider is built.
- `Page` defaults to `ViewerPage.None`.


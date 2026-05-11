using PyTrain.Libraries.Viewer.Common.Enums;

namespace PyTrain.Web.Client.Models.Messages;
internal class DeviceContentWindowStateMessage(Guid windowId, WindowState state)
{
  public WindowState State { get; } = state;
  public Guid WindowId { get; } = windowId;
}

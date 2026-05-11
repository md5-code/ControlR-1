using System.Collections.Immutable;
using System.Drawing;
using PyTrain.DesktopClient.Common.Models;
using PyTrain.Libraries.Shared.Primitives;

namespace PyTrain.DesktopClient.Common.ServiceInterfaces;

public interface IDisplayManager
{
  bool IsPrivacyScreenEnabled
  {
    get => false;
  }
  Task<ImmutableList<DisplayInfo>> GetDisplays();
  Task<DisplayInfo?> GetPrimaryDisplay();
  Task<Rectangle> GetVirtualScreenLayoutBounds();
  Task ReloadDisplays();
  Task<Result> SetPrivacyScreen(bool isEnabled);
  Task<Result<DisplayInfo>> TryFindDisplay(string deviceName);
}
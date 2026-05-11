using PyTrain.DesktopClient.Common;
using PyTrain.DesktopClient.Common.ServiceInterfaces;
using PyTrain.DesktopClient.ViewModels;
using PyTrain.DesktopClient.ViewModels.Linux;

namespace PyTrain.DesktopClient.Linux.Services;

public class LinuxNavigationItemProvider(IDesktopEnvironmentDetector desktopEnvironmentDetector) : INavigationItemProvider
{
  private readonly IDesktopEnvironmentDetector _desktopEnvironmentDetector = desktopEnvironmentDetector;

  public IEnumerable<NavigationItemDescriptor> GetNavigationItems()
  {
    var viewModelType = _desktopEnvironmentDetector.IsWayland()
      ? typeof(IPermissionsViewModelWayland)
      : typeof(IPermissionsViewModel);

    return
    [
      new(viewModelType, "shield_keyhole_regular", Localization.Permissions, 100)
    ];
  }
}

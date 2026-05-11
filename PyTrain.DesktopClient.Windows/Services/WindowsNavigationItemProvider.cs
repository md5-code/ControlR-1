using PyTrain.DesktopClient.Common;
using PyTrain.DesktopClient.Common.ServiceInterfaces;
using PyTrain.DesktopClient.ViewModels;

namespace PyTrain.DesktopClient.Windows.Services;

public class WindowsNavigationItemProvider : INavigationItemProvider
{
  public IEnumerable<NavigationItemDescriptor> GetNavigationItems()
  {
    return
    [
      new(typeof(IPermissionsViewModel), "shield_keyhole_regular", Localization.Permissions, 100)
    ];
  }
}

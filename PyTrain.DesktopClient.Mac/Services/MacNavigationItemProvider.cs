using PyTrain.DesktopClient.Common;
using PyTrain.DesktopClient.Common.ServiceInterfaces;
using PyTrain.DesktopClient.ViewModels.Mac;

namespace PyTrain.DesktopClient.Mac.Services;

public class MacNavigationItemProvider : INavigationItemProvider
{
  public IEnumerable<NavigationItemDescriptor> GetNavigationItems()
  {
    return
    [
      new(typeof(IPermissionsViewModelMac), "shield_keyhole_regular", Localization.Permissions, 100)
    ];
  }
}

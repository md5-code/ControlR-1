namespace PyTrain.DesktopClient.Common.ServiceInterfaces;

public interface INavigationItemProvider
{
  IEnumerable<NavigationItemDescriptor> GetNavigationItems();
}

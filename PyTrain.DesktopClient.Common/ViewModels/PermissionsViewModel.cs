using PyTrain.DesktopClient.Common.ViewModelInterfaces;
using PyTrain.DesktopClient.Views;

namespace PyTrain.DesktopClient.ViewModels;

public interface IPermissionsViewModel : IViewModelBase
{
}

public class PermissionsViewModel : ViewModelBase<PermissionsView>, IPermissionsViewModel
{
}

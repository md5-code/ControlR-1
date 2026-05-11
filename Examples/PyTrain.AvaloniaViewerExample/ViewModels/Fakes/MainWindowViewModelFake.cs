using CommunityToolkit.Mvvm.ComponentModel;
using PyTrain.Libraries.Viewer.Common.Options;
using PyTrain.Viewer.Avalonia.Services.Navigation;

namespace PyTrain.AvaloniaViewerExample.ViewModels.Fakes;

public partial class MainWindowViewModelFake : ObservableObject, IMainWindowViewModel
{
  [ObservableProperty]
  private ViewerPage _activePage = ViewerPage.RemoteControl;

  [ObservableProperty]
  private bool _isDarkMode = true;

  public PyTrainViewerOptions ViewerOptions { get; } = new()
  {
    BaseUrl = new Uri("https://pytrain.example.com"),
    DeviceId = Guid.NewGuid(),
    PersonalAccessToken = "fake-token"
  };
}

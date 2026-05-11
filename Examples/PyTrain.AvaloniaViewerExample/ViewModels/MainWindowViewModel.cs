using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using PyTrain.Libraries.Viewer.Common.Options;
using PyTrain.Viewer.Avalonia.Services.Navigation;

namespace PyTrain.AvaloniaViewerExample.ViewModels;

public interface IMainWindowViewModel : INotifyPropertyChanged
{
  ViewerPage ActivePage { get; set; }
  bool IsDarkMode { get; set; }
  PyTrainViewerOptions ViewerOptions { get; }
}

public partial class MainWindowViewModel(PyTrainViewerOptions viewerOptions) : ObservableObject, IMainWindowViewModel
{
  [ObservableProperty]
  public partial ViewerPage ActivePage { get; set; } = ViewerPage.RemoteControl;
  
  [ObservableProperty]
  public partial bool IsDarkMode { get; set; } = true;
  public PyTrainViewerOptions ViewerOptions { get; } = viewerOptions;
}

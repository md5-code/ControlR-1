using Avalonia.Controls;

namespace PyTrain.Viewer.Avalonia.Views;

public partial class RemoteControlView : UserControl
{
    public RemoteControlView()
    {
        InitializeComponent();
    }

    public RemoteControlView(RemoteControlViewModel viewModel) : this()
    {
        DataContext = viewModel;
    }
}

using PyTrain.Libraries.Avalonia.Controls.Dialogs;
using PyTrain.Libraries.Avalonia.ViewModels;
using PyTrain.Viewer.Avalonia.Views.Dialogs;

namespace PyTrain.Viewer.Avalonia.ViewModels.Dialogs;

public interface ITerminalKeyboardShortcutsDialogViewModel : IDisposable, IViewReference<TerminalKeyboardShortcutsDialogView>
{
  IRelayCommand CloseCommand { get; }
}

public partial class TerminalKeyboardShortcutsDialogViewModel(
  IDialogProvider dialogProvider) : ViewModelBase<TerminalKeyboardShortcutsDialogView>, ITerminalKeyboardShortcutsDialogViewModel
{
  private readonly IDialogProvider _dialogProvider = dialogProvider;

  [RelayCommand]
  private void Close()
  {
    _dialogProvider.Close();
  }
}
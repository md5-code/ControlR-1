using PyTrain.DesktopClient.Controls.Dialogs;

namespace PyTrain.DesktopClient.Services;

public interface IDialogProvider
{
  Task<MessageBoxResult> ShowMessageBox(string title, string message, MessageBoxButtons messageBoxButtons);
}

internal class DialogProvider : IDialogProvider
{
  public async Task<MessageBoxResult> ShowMessageBox(string title, string message, MessageBoxButtons messageBoxButtons)
  {
    return await MessageBox.Show(
      title,
      message,
      messageBoxButtons);
  }
}
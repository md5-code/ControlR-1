namespace PyTrain.DesktopClient.Common.ServiceInterfaces;

public interface IClipboardManager
{
  Task<string?> GetText();

  Task SetText(string? text);
}

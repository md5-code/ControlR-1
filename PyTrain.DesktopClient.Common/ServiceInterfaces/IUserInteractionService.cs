namespace PyTrain.DesktopClient.Common.ServiceInterfaces;

public interface IUserInteractionService
{
  Task<bool> ShowConsentDialogAsync(string requesterName, CancellationToken cancellationToken);
}

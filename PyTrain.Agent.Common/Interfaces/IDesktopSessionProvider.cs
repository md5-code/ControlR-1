using PyTrain.Libraries.Api.Contracts.Dtos.Devices;

namespace PyTrain.Agent.Common.Interfaces;
public interface IDesktopSessionProvider
{
  Task<DesktopSession[]> GetActiveDesktopClients();
  Task<string[]> GetLoggedInUsers();
}

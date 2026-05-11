using PyTrain.Agent.Shared.Models;

namespace PyTrain.Agent.Shared.Interfaces;

public interface IAgentInstaller
{
  Task Install(AgentInstallRequest request);

  Task Uninstall();
}

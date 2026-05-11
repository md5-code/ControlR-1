using Microsoft.AspNetCore.SignalR.Client;

namespace PyTrain.Agent.LoadTester;

public class TestAgentRetryPolicy : IRetryPolicy
{
  public TimeSpan? NextRetryDelay(RetryContext retryContext)
  {
    return TimeSpan.FromSeconds(10);
  }
}

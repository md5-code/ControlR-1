using PyTrain.Agent.Shared.Services;
using Microsoft.Extensions.Hosting;

namespace PyTrain.Agent.LoadTester;
internal class FakeCpuUtilizationSampler : ICpuUtilizationSampler, IHostedService
{
  public double CurrentUtilization { get; } = 0;

  public Task StartAsync(CancellationToken cancellationToken)
  {
    return Task.CompletedTask;
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    return Task.CompletedTask;
  }
}

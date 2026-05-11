using PyTrain.Agent.Shared.Startup;
using Microsoft.Extensions.Hosting;

namespace PyTrain.Agent.Shared.Tests;

public class DependencyResolutionTests
{
  [Fact]
  internal void Build_InDevelopment_ValidatesDependencyGraph()
  {
    var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
    {
      EnvironmentName = Environments.Development
    });

    builder.AddPyTrainInstallerServices(instanceId: null, serverUri: new Uri("https://example.invalid"), loadAppSettings: false);

    using var host = builder.Build();
  }

  [Fact]
  internal void Build_InProduction_Succeeds()
  {
    var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
    {
      EnvironmentName = Environments.Production
    });

    builder.AddPyTrainInstallerServices(instanceId: null, serverUri: new Uri("https://example.invalid"), loadAppSettings: false);

    using var host = builder.Build();
  }
}
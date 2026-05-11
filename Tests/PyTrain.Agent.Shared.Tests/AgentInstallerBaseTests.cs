using PyTrain.Agent.Shared.Interfaces;
using PyTrain.Agent.Shared.Models;
using PyTrain.Agent.Shared.Options;
using PyTrain.Agent.Shared.Services;
using PyTrain.Agent.Shared.Services.Base;
using PyTrain.ApiClient;
using PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;
using PyTrain.Libraries.Shared.Constants;
using PyTrain.Libraries.Shared.Primitives;
using PyTrain.Libraries.Shared.Services;
using PyTrain.Libraries.Shared.Services.FileSystem;
using PyTrain.Libraries.Shared.Services.Processes;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace PyTrain.Agent.Shared.Tests;

public class AgentInstallerBaseTests
{
  [Fact]
  public void GetInstanceInstallDirectory_WhenInstanceIdMissing_UsesDefaultSubdirectory()
  {
    var result = TestAgentInstaller.GetInstallDirectoryForTest(@"C:\Program Files\PyTrain", instanceId: null);

    Assert.Equal(Path.Combine(@"C:\Program Files\PyTrain", AppConstants.DefaultInstallDirectoryName), result);
  }

  [Fact]
  public void StopProcesses_KillsOnlyProcessesForCurrentInstallDirectory()
  {
    var currentAgentProcessId = Environment.ProcessId;
    var targetAgentPath = @"C:\Program Files\PyTrain\default\PyTrain.Agent.exe";
    var targetDesktopClientPath = @"C:\Program Files\PyTrain\default\DesktopClient\PyTrain.DesktopClient.exe";
    var matchingAgent = CreateProcess(101, targetAgentPath);
    var currentAgent = CreateProcess(currentAgentProcessId, targetAgentPath);
    var otherAgent = CreateProcess(102, @"C:\Program Files\PyTrain\c.jaredg.dev\PyTrain.Agent.exe");
    var matchingDesktop = CreateProcess(201, targetDesktopClientPath);
    var otherDesktop = CreateProcess(202, @"C:\Program Files\PyTrain\c.jaredg.dev\DesktopClient\PyTrain.DesktopClient.exe");
    var processManager = new Mock<IProcessManager>();

    processManager
      .Setup(x => x.GetProcessesByName("PyTrain.Agent"))
      .Returns([matchingAgent.Object, currentAgent.Object, otherAgent.Object]);

    processManager
      .Setup(x => x.GetProcessesByName("PyTrain.DesktopClient"))
      .Returns([matchingDesktop.Object, otherDesktop.Object]);

    var systemEnvironment = new Mock<ISystemEnvironment>();
    systemEnvironment.SetupGet(x => x.ProcessId).Returns(currentAgentProcessId);

    var sut = new TestAgentInstaller(processManager.Object, systemEnvironment.Object);

    var result = sut.StopProcessesForTest(targetAgentPath, targetDesktopClientPath);

    Assert.True(result.IsSuccess);
    matchingAgent.Verify(x => x.Kill(), Times.Once);
    currentAgent.Verify(x => x.Kill(), Times.Never);
    otherAgent.Verify(x => x.Kill(), Times.Never);
    matchingDesktop.Verify(x => x.Kill(), Times.Once);
    otherDesktop.Verify(x => x.Kill(), Times.Never);
  }

  private static Mock<IProcess> CreateProcess(int processId, string filePath)
  {
    var process = new Mock<IProcess>();
    process.SetupGet(x => x.Id).Returns(processId);
    process.SetupGet(x => x.FilePath).Returns(filePath);
    return process;
  }

  private sealed class TestAgentInstaller(IProcessManager processManager, ISystemEnvironment systemEnvironment)
    : AgentInstallerBase(
      Mock.Of<IFileSystem>(),
      Mock.Of<IFileSystemPathProvider>(),
      Mock.Of<IPyTrainApi>(),
      Mock.Of<IDeviceInfoProvider>(),
      Mock.Of<IOptionsAccessor>(),
      processManager,
      systemEnvironment,
      Mock.Of<IOptionsMonitor<AgentAppOptions>>(),
      NullLogger<AgentInstallerBase>.Instance)
  {
    public static string GetInstallDirectoryForTest(string rootDirectory, string? instanceId)
    {
      return GetInstanceInstallDirectory(rootDirectory, instanceId);
    }

    public Result StopProcessesForTest(string targetAgentPath, string targetDesktopClientPath)
    {
      return StopProcesses(targetAgentPath, targetDesktopClientPath);
    }
  }
}
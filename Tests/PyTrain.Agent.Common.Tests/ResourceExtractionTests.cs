using PyTrain.Libraries.Shared.Services;
using PyTrain.Agent.Shared.Services;
using System.Reflection;
using PyTrain.Libraries.Shared.Helpers;

namespace PyTrain.Agent.Common.Tests;

public class ResourceExtractionTests
{
  private readonly EmbeddedResourceAccessor _accessor = new();
  private readonly Assembly _assembly = typeof(FileSystemPathProvider).Assembly;

  [Fact]
  public async Task GetResourceAsString_ForLaunchAgent_ReturnsExpectedString()
  {
    // Arrange
    var solutionDirResult = IoHelper.GetSolutionDir(Directory.GetCurrentDirectory());
    Assert.True(solutionDirResult.IsSuccess);
    var resourcePath = Path.Combine(solutionDirResult.Value, "PyTrain.Agent.Shared", "Resources", "LaunchAgent.plist");
    var expected = await File.ReadAllTextAsync(resourcePath, TestContext.Current.CancellationToken);

    // Act
    var actual = await _accessor.GetResourceAsString(_assembly, "LaunchAgent.plist");

    // Assert
    Assert.Equal(expected, actual);
  }

  [Fact]
  public async Task GetResourceAsString_ForLaunchDaemon_ReturnsExpectedString()
  {
    // Arrange
    var solutionDirResult = IoHelper.GetSolutionDir(Directory.GetCurrentDirectory());
    Assert.True(solutionDirResult.IsSuccess);
    var resourcePath =
      Path.Combine(solutionDirResult.Value, "PyTrain.Agent.Shared", "Resources", "LaunchDaemon.plist");
    var expected = await File.ReadAllTextAsync(resourcePath, TestContext.Current.CancellationToken);
    // Act
    var actual = await _accessor.GetResourceAsString(_assembly, "LaunchDaemon.plist");
    // Assert
    Assert.Equal(expected, actual);
  }

  [Fact]
  public async Task GetResourceAsString_ForLinuxAgentService_ReturnsExpectedString()
  {
    // Arrange
    var solutionDirResult = IoHelper.GetSolutionDir(Directory.GetCurrentDirectory());
    Assert.True(solutionDirResult.IsSuccess);
    var resourcePath = Path.Combine(solutionDirResult.Value, "PyTrain.Agent.Shared", "Resources",
      "pytrain.agent.service");
    var expected = await File.ReadAllTextAsync(resourcePath, TestContext.Current.CancellationToken);

    // Act
    var actual =
      await _accessor.GetResourceAsString(_assembly, "pytrain.agent.service");

    // Assert
    Assert.Equal(expected, actual);
  }

  [Fact]
  public async Task GetResourceAsString_ForLinuxDesktopService_ReturnsExpectedString()
  {
    // Arrange
    var solutionDirResult = IoHelper.GetSolutionDir(Directory.GetCurrentDirectory());
    Assert.True(solutionDirResult.IsSuccess);
    var resourcePath = Path.Combine(solutionDirResult.Value, "PyTrain.Agent.Shared", "Resources", "pytrain.desktop.service");
    var expected = await File.ReadAllTextAsync(resourcePath, TestContext.Current.CancellationToken);

    // Act
    var actual =
      await _accessor.GetResourceAsString(_assembly, "pytrain.desktop.service");

    // Assert
    Assert.Equal(expected, actual);
  }
}
using System.Runtime.Versioning;
using PyTrain.Libraries.TestingUtilities;

namespace PyTrain.DesktopClient.Windows.Tests;

[SupportedOSPlatform("windows8.0")]
public class PathConstantsTests
{
  [WindowsOnlyFact]
  public void GetLogsPath_WhenInstanceIdMissing_UsesDefaultDirectory()
  {
    var result = PathConstants.GetLogsPath(instanceId: null);

    Assert.Contains(@"\PyTrain\", result, StringComparison.OrdinalIgnoreCase);
    Assert.EndsWith(@"default\Logs\PyTrain.DesktopClient\LogFile.log", result, StringComparison.OrdinalIgnoreCase);
  }
}
using PyTrain.Libraries.TestingUtilities;

namespace PyTrain.DesktopClient.Mac.Tests;

public class PlatformTest
{
    [MacOnlyFact]
    public void PlatformRunTest()
    {
      Assert.True(true, "MacOS tests ran successfully.");
    }
}
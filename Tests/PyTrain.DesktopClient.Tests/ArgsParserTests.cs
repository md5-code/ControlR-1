using PyTrain.DesktopClient.Common.Startup;

namespace PyTrain.DesktopClient.Tests;

public class ArgsParserTests
{
  [Fact]
  public void GetArgValue_WhenFlagHasNoValueAndBoolRequested_ReturnsTrue()
  {
    var args = ArgsParser.ParseArgs(["PyTrain.DesktopClient.exe", "--enable-feature"]);

    var result = ArgsParser.GetArgValue(args, "enable-feature", defaultValue: false);

    Assert.True(result);
  }

  [Fact]
  public void GetArgValue_WhenFlagHasNoValueAndStringRequested_ReturnsEmptyString()
  {
    var args = ArgsParser.ParseArgs(["PyTrain.DesktopClient.exe", "--instance-id"]);

    var result = ArgsParser.GetArgValue<string?>(args, "instance-id", defaultValue: string.Empty);

    Assert.Equal(string.Empty, result);
  }
}
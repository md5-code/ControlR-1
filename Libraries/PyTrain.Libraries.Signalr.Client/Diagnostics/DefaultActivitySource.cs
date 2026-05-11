using System.Diagnostics;

namespace PyTrain.Libraries.Signalr.Client.Diagnostics;
internal static class DefaultActivitySource
{
  public const string Name = "PyTrain.Libraries.Signalr.Client";
  public static readonly ActivitySource Instance = new(Name);
  public static Activity? StartActivity(string name) => Instance.StartActivity(name);
}

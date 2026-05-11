using PyTrain.ApiClient;
using PyTrain.Viewer.Avalonia.Services.Navigation;
using Microsoft.Extensions.DependencyInjection;

namespace PyTrain.Viewer.Avalonia.Services;


/// <summary>
/// Information about a registered viewer instance.
/// </summary>
public record ViewerInstanceInfo(Guid InstanceId, PyTrainViewer Viewer, IServiceProvider ServiceProvider)
{
  public IHubConnection<IViewerHub> GetHubConnection() => ServiceProvider.GetRequiredService<IHubConnection<IViewerHub>>();
  public IPyTrainApi GetPyTrainApi() => ServiceProvider.GetRequiredService<IPyTrainApi>();
  public INavigator GetNavigator() => ServiceProvider.GetRequiredService<INavigator>();
}
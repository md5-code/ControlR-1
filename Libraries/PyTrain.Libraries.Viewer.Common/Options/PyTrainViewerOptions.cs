namespace PyTrain.Libraries.Viewer.Common.Options;

/// <summary>
///   Required options for configuring a PyTrainViewer instance.
/// </summary>
public class PyTrainViewerOptions
{
  public const string PersonalAccessTokenHeaderName = "x-personal-token";
  
  /// <summary>
  ///   The base URL of the PyTrain server to which the viewer will connect (e.g. "https://pytrain.example.com").
  /// </summary>
  public required Uri BaseUrl { get; set; }

  /// <summary>
  ///   The device ID that the viewer will be accessing.
  /// </summary>
  public required Guid DeviceId { get; set; }
  
  /// <summary>
  ///   The PAT for the current user, which should be created within PyTrain.
  /// </summary>
  public required string PersonalAccessToken { get; set; }
}
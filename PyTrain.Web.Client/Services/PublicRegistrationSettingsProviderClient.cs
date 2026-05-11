namespace PyTrain.Web.Client.Services;

internal class PublicRegistrationSettingsProviderClient(
  IPyTrainApi pytrainApi,
  ILogger<PublicRegistrationSettingsProviderClient> logger) : IPublicRegistrationSettingsProvider
{
  private readonly IPyTrainApi _pytrainApi = pytrainApi;
  private readonly ILogger<PublicRegistrationSettingsProviderClient> _logger = logger;
  private bool? _cachedValue;

  public async Task<bool> GetIsPublicRegistrationEnabled()
  {
    if (_cachedValue.HasValue)
    {
      return _cachedValue.Value;
    }

    try
    {
      var result = await _pytrainApi.PublicRegistrationSettings.GetPublicRegistrationSettings();
      if (result.IsSuccess)
      {
        _cachedValue = result.Value.IsPublicRegistrationEnabled;
        return _cachedValue.Value;
      }

      _logger.LogError("Failed to get public registration settings: {Reason}", result.Reason);
      return false;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error while getting public registration settings.");
      return false;
    }
  }
}

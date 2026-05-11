using System.Net.Http.Json;
using PyTrain.Libraries.Api.Contracts.Constants;
using PyTrain.Libraries.Api.Contracts.Dtos;
using PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;

namespace PyTrain.ApiClient;

public partial class PyTrainApi
{
  async Task<ApiResult<PublicRegistrationSettings>> IPublicRegistrationSettingsApi.GetPublicRegistrationSettings(CancellationToken cancellationToken)
  {
    return await ExecuteApiCall(async () =>
      await _client.GetFromJsonAsync<PublicRegistrationSettings>(HttpConstants.PublicRegistrationSettingsEndpoint, cancellationToken));
  }
}

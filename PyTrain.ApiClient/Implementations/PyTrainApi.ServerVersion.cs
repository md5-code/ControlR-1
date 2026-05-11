using System.Net.Http.Json;
using PyTrain.Libraries.Api.Contracts.Constants;
using PyTrain.Libraries.Api.Contracts.Dtos;

namespace PyTrain.ApiClient;

public partial class PyTrainApi
{
  async Task<ApiResult<Version>> IServerVersionApi.GetCurrentServerVersion(CancellationToken cancellationToken)
  {
    return await ExecuteApiCall(async () =>
      await _client.GetFromJsonAsync<Version>(HttpConstants.ServerVersionEndpoint, cancellationToken));
  }
}

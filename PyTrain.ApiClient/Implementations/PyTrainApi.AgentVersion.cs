using System.Net.Http.Json;
using PyTrain.Libraries.Api.Contracts.Constants;
using PyTrain.Libraries.Api.Contracts.Dtos;
using Microsoft.Extensions.Logging;

namespace PyTrain.ApiClient;

public partial class PyTrainApi
{
  async Task<ApiResult<Version>> IAgentVersionApi.GetCurrentAgentVersion(CancellationToken cancellationToken)
  {
    return await ExecuteApiCall(async () =>
    {
      var version = await _client.GetFromJsonAsync<Version>(HttpConstants.AgentVersionEndpoint, cancellationToken);
      _logger.LogInformation("Latest Agent version on server: {LatestAgentVersion}", version);
      return version;
    });
  }
}

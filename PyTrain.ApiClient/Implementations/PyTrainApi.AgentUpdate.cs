using PyTrain.Libraries.Api.Contracts.Constants;
using PyTrain.Libraries.Api.Contracts.Dtos;
using PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;
using PyTrain.Libraries.Api.Contracts.Enums;
using System.Net.Http.Json;

namespace PyTrain.ApiClient;

public partial class PyTrainApi
{
  async Task<ApiResult<BundleMetadataDto>> IAgentUpdateApi.GetBundleMetadata(RuntimeId runtime, CancellationToken cancellationToken)
  {
    return await ExecuteApiCall(async () =>
      await _client.GetFromJsonAsync<BundleMetadataDto>(
        $"{HttpConstants.AgentUpdateEndpoint}/get-bundle-metadata/{runtime}",
        cancellationToken));
  }
}

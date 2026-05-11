using PyTrain.Libraries.Api.Contracts.Constants;
using PyTrain.Libraries.Api.Contracts.Dtos;

namespace PyTrain.ApiClient;

public partial class PyTrainApi
{
  async Task<ApiResult> ITestEmailApi.SendTestEmail(CancellationToken cancellationToken)
  {
    return await ExecuteApiCall(async () =>
    {
      using var response = await _client.PostAsync(HttpConstants.TestEmailEndpoint, null, cancellationToken);
      await response.EnsureSuccessStatusCodeWithDetails();
    });
  }
}

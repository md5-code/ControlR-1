using PyTrain.Libraries.Api.Contracts.Constants;
using PyTrain.Libraries.Api.Contracts.Dtos;

namespace PyTrain.ApiClient;

public partial class PyTrainApi
{
  async Task<ApiResult<byte[]>> IDesktopPreviewApi.GetDesktopPreview(Guid deviceId, int targetProcessId, CancellationToken cancellationToken)
  {
    return await ExecuteApiCall(async () =>
    {
      using var response = await _client.GetAsync($"{HttpConstants.DesktopPreviewEndpoint}/{deviceId}/{targetProcessId}", cancellationToken);
      await response.EnsureSuccessStatusCodeWithDetails();
      return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    });
  }
}

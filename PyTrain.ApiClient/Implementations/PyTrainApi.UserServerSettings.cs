using System.Net.Http.Json;
using PyTrain.Libraries.Api.Contracts.Constants;
using PyTrain.Libraries.Api.Contracts.Dtos;
using PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;

namespace PyTrain.ApiClient;

public partial class PyTrainApi
{
  async Task<ApiResult<long>> IUserServerSettingsApi.GetFileUploadMaxSize(CancellationToken cancellationToken)
  {
    return await ExecuteApiCall(async () =>
    {
      using var response = await _client.GetAsync($"{HttpConstants.UserServerSettingsEndpoint}/file-upload-max-size", cancellationToken);
      await response.EnsureSuccessStatusCodeWithDetails();
      var dto = await response.Content.ReadFromJsonAsync<FileUploadMaxSizeResponseDto>(cancellationToken)
        ?? throw new HttpRequestException("The server response was empty.");
      return dto.MaxFileSize;
    });
  }
}

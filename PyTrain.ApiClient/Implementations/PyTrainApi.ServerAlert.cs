using System.Net;
using System.Net.Http.Json;
using PyTrain.Libraries.Api.Contracts.Constants;
using PyTrain.Libraries.Api.Contracts.Dtos;
using PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;

namespace PyTrain.ApiClient;

public partial class PyTrainApi
{
  async Task<ApiResult<ServerAlertResponseDto>> IServerAlertApi.GetServerAlert(CancellationToken cancellationToken)
  {
    return await ExecuteApiCall(async () =>
    {
      using var response = await _client.GetAsync(HttpConstants.ServerAlertEndpoint, cancellationToken);
      if (response.StatusCode is HttpStatusCode.NoContent or HttpStatusCode.NotFound)
      {
        return ServerAlertResponseDto.Empty;
      }
      await response.EnsureSuccessStatusCodeWithDetails();
      return await response.Content.ReadFromJsonAsync<ServerAlertResponseDto>(cancellationToken)
        ?? throw new HttpRequestException("The server response was empty.", null, response.StatusCode);
    });
  }

  async Task<ApiResult<ServerAlertResponseDto>> IServerAlertApi.UpdateServerAlert(ServerAlertRequestDto request, CancellationToken cancellationToken)
  {
    return await ExecuteApiCall(async () =>
    {
      using var response = await _client.PostAsJsonAsync(HttpConstants.ServerAlertEndpoint, request, cancellationToken);
      await response.EnsureSuccessStatusCodeWithDetails();
      return await response.Content.ReadFromJsonAsync<ServerAlertResponseDto>(cancellationToken);
    });
  }
}

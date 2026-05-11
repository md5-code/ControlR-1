using System.Net.Http.Json;
using PyTrain.Libraries.Api.Contracts.Constants;
using PyTrain.Libraries.Api.Contracts.Dtos;

namespace PyTrain.ApiClient;

public partial class PyTrainApi
{
  async Task<ApiResult> IAuthApi.LogOut(CancellationToken cancellationToken)
  {
    return await ExecuteApiCall(async () =>
    {
      using var response = await _client.PostAsJsonAsync($"{HttpConstants.AuthEndpoint}/logout", new { }, cancellationToken);
      await response.EnsureSuccessStatusCodeWithDetails();
    });
  }
}

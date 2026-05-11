using System.Net;
using System.Net.Http.Json;
using PyTrain.Libraries.Api.Contracts.Constants;
using PyTrain.Libraries.Api.Contracts.Dtos;
using PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;

namespace PyTrain.ApiClient;

public partial class PyTrainApi
{
  async Task<ApiResult<TenantResponseDto[]>> ITenantsApi.GetAllTenants(CancellationToken cancellationToken)
  {
    return await ExecuteApiCall(async () =>
      await _client.GetFromJsonAsync<TenantResponseDto[]>(HttpConstants.TenantsEndpoint, cancellationToken));
  }

  async Task<ApiResult<TenantResponseDto>> ITenantsApi.CreateTenant(
    CreateTenantRequestDto request,
    CancellationToken cancellationToken)
  {
    return await ExecuteApiCall(async () =>
    {
      using var response = await _client.PostAsJsonAsync(HttpConstants.TenantsEndpoint, request, cancellationToken);
      if (response.StatusCode == HttpStatusCode.Conflict)
      {
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new HttpRequestException(
          string.IsNullOrWhiteSpace(content) ? "A tenant or user with that email already exists." : content,
          inner: null,
          HttpStatusCode.Conflict);
      }
      await response.EnsureSuccessStatusCodeWithDetails();
      return await response.Content.ReadFromJsonAsync<TenantResponseDto>(cancellationToken);
    });
  }
}

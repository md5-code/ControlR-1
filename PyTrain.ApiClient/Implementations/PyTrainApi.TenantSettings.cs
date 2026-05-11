using System.Net;
using System.Net.Http.Json;
using PyTrain.Libraries.Api.Contracts.Constants;
using PyTrain.Libraries.Api.Contracts.Dtos;
using PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;

namespace PyTrain.ApiClient;

public partial class PyTrainApi
{
  async Task<ApiResult> ITenantSettingsApi.DeleteTenantSetting(string settingName, CancellationToken cancellationToken)
  {
    return await ExecuteApiCall(async () =>
    {
      var url = $"{HttpConstants.TenantSettingsEndpoint}/{settingName}";
      using var response = await _client.DeleteAsync(url, cancellationToken);
      await response.EnsureSuccessStatusCodeWithDetails();
    });
  }

  async Task<ApiResult<TenantSettingResponseDto>> ITenantSettingsApi.GetTenantSetting(string settingName, CancellationToken cancellationToken)
  {
    return await ExecuteApiCall(async () =>
    {
      using var response = await _client.GetAsync($"{HttpConstants.TenantSettingsEndpoint}/{settingName}", cancellationToken);
      if (response.StatusCode == HttpStatusCode.NoContent)
      {
        return new TenantSettingResponseDto(Id: null, Name: settingName, Value: null);
      }

      await response.EnsureSuccessStatusCodeWithDetails();
      return await response.Content.ReadFromJsonAsync<TenantSettingResponseDto>(cancellationToken)
        ?? throw new HttpRequestException("The server response was empty.", null, response.StatusCode);
    });
  }

  async Task<ApiResult<TenantSettingsDto>> ITenantSettingsApi.GetTenantSettings(CancellationToken cancellationToken)
  {
    return await ExecuteApiCall(async () =>
      await _client.GetFromJsonAsync<TenantSettingsDto>(HttpConstants.TenantSettingsEndpoint, cancellationToken));
  }

  async Task<ApiResult<TenantSettingResponseDto>> ITenantSettingsApi.SetTenantSetting(TenantSettingRequestDto request, CancellationToken cancellationToken)
  {
    return await ExecuteApiCall(async () =>
    {
      using var response = await _client.PostAsJsonAsync(HttpConstants.TenantSettingsEndpoint, request, cancellationToken);
      await response.EnsureSuccessStatusCodeWithDetails();
      return await response.Content.ReadFromJsonAsync<TenantSettingResponseDto>(cancellationToken);
    });
  }

  async Task<ApiResult<TenantSettingsDto>> ITenantSettingsApi.SetTenantSettings(TenantSettingsDto request, CancellationToken cancellationToken)
  {
    return await ExecuteApiCall(async () =>
    {
      using var response = await _client.PutAsJsonAsync(HttpConstants.TenantSettingsEndpoint, request, cancellationToken);
      await response.EnsureSuccessStatusCodeWithDetails();
      return await response.Content.ReadFromJsonAsync<TenantSettingsDto>(cancellationToken);
    });
  }
}

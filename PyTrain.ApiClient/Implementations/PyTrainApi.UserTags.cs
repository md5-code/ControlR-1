using System.Net.Http.Json;
using PyTrain.Libraries.Api.Contracts.Constants;
using PyTrain.Libraries.Api.Contracts.Dtos;
using PyTrain.Libraries.Api.Contracts.Dtos.HubDtos;
using PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;

namespace PyTrain.ApiClient;

public partial class PyTrainApi
{
  async Task<ApiResult> IUserTagsApi.AddUserTag(UserTagAddRequestDto request, CancellationToken cancellationToken)
  {
    return await ExecuteApiCall(async () =>
    {
      using var response = await _client.PostAsJsonAsync($"{HttpConstants.UserTagsEndpoint}", request, cancellationToken);
      await response.EnsureSuccessStatusCodeWithDetails();
    });
  }

  async Task<ApiResult<TagResponseDto[]>> IUserTagsApi.GetAllowedTags(CancellationToken cancellationToken)
  {
    return await ExecuteApiCall(async () =>
      await _client.GetFromJsonAsync<TagResponseDto[]>(HttpConstants.UserTagsEndpoint, cancellationToken));
  }

  async Task<ApiResult<TagResponseDto[]>> IUserTagsApi.GetUserTags(Guid userId, CancellationToken cancellationToken)
  {
    return await ExecuteApiCall(async () =>
      await _client.GetFromJsonAsync<TagResponseDto[]>(
        $"{HttpConstants.UserTagsEndpoint}/{userId}",
        cancellationToken));
  }

  async Task<ApiResult> IUserTagsApi.RemoveUserTag(Guid userId, Guid tagId, CancellationToken cancellationToken)
  {
    return await ExecuteApiCall(async () =>
    {
      using var response = await _client.DeleteAsync($"{HttpConstants.UserTagsEndpoint}/{userId}/{tagId}", cancellationToken);
      await response.EnsureSuccessStatusCodeWithDetails();
    });
  }
}

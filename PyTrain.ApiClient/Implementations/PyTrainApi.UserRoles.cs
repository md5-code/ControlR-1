using System.Net.Http.Json;
using PyTrain.Libraries.Api.Contracts.Constants;
using PyTrain.Libraries.Api.Contracts.Dtos;
using PyTrain.Libraries.Api.Contracts.Dtos.HubDtos;
using PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;

namespace PyTrain.ApiClient;

public partial class PyTrainApi
{
  async Task<ApiResult> IUserRolesApi.AddUserRole(UserRoleAddRequestDto request, CancellationToken cancellationToken)
  {
    return await ExecuteApiCall(async () =>
    {
      using var response = await _client.PostAsJsonAsync($"{HttpConstants.UserRolesEndpoint}", request, cancellationToken);
      await response.EnsureSuccessStatusCodeWithDetails();
    });
  }

  async Task<ApiResult<RoleResponseDto[]>> IUserRolesApi.GetOwnRoles(CancellationToken cancellationToken)
  {
    return await ExecuteApiCall(async () =>
      await _client.GetFromJsonAsync<RoleResponseDto[]>(HttpConstants.UserRolesEndpoint, cancellationToken));
  }

  async Task<ApiResult<RoleResponseDto[]>> IUserRolesApi.GetUserRoles(Guid userId, CancellationToken cancellationToken)
  {
    return await ExecuteApiCall(async () =>
      await _client.GetFromJsonAsync<RoleResponseDto[]>($"{HttpConstants.UserRolesEndpoint}/{userId}", cancellationToken));
  }

  async Task<ApiResult> IUserRolesApi.RemoveUserRole(Guid userId, Guid roleId, CancellationToken cancellationToken)
  {
    return await ExecuteApiCall(async () =>
    {
      using var response = await _client.DeleteAsync($"{HttpConstants.UserRolesEndpoint}/{userId}/{roleId}", cancellationToken);
      await response.EnsureSuccessStatusCodeWithDetails();
    });
  }
}

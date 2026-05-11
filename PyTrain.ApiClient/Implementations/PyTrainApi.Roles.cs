using System.Net.Http.Json;
using PyTrain.Libraries.Api.Contracts.Constants;
using PyTrain.Libraries.Api.Contracts.Dtos;
using PyTrain.Libraries.Api.Contracts.Dtos.HubDtos;
using PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;

namespace PyTrain.ApiClient;

public partial class PyTrainApi
{
  async Task<ApiResult<RoleResponseDto[]>> IRolesApi.GetAllRoles(CancellationToken cancellationToken)
  {
    return await ExecuteApiCall(async () =>
      await _client.GetFromJsonAsync<RoleResponseDto[]>(HttpConstants.RolesEndpoint, cancellationToken));
  }
}

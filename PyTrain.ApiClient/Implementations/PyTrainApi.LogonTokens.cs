using System.Net.Http.Json;
using PyTrain.Libraries.Api.Contracts.Constants;
using PyTrain.Libraries.Api.Contracts.Dtos;
using PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;

namespace PyTrain.ApiClient;

public partial class PyTrainApi
{
	async Task<ApiResult<LogonTokenResponseDto>> ILogonTokensApi.CreateLogonToken(LogonTokenRequestDto request, CancellationToken cancellationToken)
	{
		return await ExecuteApiCall(async () =>
		{
			using var response = await _client.PostAsJsonAsync(HttpConstants.LogonTokensEndpoint, request, cancellationToken);
     await response.EnsureSuccessStatusCodeWithDetails();
			return await response.Content.ReadFromJsonAsync<LogonTokenResponseDto>(cancellationToken);
		});
	}
}

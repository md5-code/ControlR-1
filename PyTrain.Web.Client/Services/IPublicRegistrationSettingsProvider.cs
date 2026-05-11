using PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;

namespace PyTrain.Web.Client.Services;

public interface IPublicRegistrationSettingsProvider
{
  Task<bool> GetIsPublicRegistrationEnabled();
}

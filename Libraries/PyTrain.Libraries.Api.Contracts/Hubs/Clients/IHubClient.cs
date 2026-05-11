using PyTrain.Libraries.Api.Contracts.Dtos.RemoteControlDtos;

namespace PyTrain.Libraries.Api.Contracts.Hubs.Clients;

public interface IHubClient
{
  Task ReceiveDto(DtoWrapper dtoWrapper);
}
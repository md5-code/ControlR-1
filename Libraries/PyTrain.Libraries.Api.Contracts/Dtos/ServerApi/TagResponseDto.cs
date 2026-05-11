using PyTrain.Libraries.Api.Contracts.Enums;

namespace PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;

public record TagResponseDto(
  Guid Id,
  string Name,
  TagType Type,
  IReadOnlyList<Guid> UserIds,
  IReadOnlyList<Guid> DeviceIds)
{
  public override string ToString() => Name;
}
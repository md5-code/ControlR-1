namespace PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;

public record CreateUserRequestDto(
  string UserName,
  string? Email,
  string? Password,
  IEnumerable<Guid>? RoleIds,
  IEnumerable<Guid>? TagIds);

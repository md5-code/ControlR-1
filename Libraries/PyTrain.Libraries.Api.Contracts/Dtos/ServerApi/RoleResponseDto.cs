namespace PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;
public record RoleResponseDto(Guid Id, string Name, IReadOnlyList<Guid> UserIds);

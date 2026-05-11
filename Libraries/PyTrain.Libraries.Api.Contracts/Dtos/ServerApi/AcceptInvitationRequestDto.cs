namespace PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;
public record AcceptInvitationRequestDto(
  string ActivationCode,
  string Email,
  string Password);

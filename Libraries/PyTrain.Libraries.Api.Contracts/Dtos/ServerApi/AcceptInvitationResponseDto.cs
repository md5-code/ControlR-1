using System.Diagnostics.CodeAnalysis;

namespace PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;
public record AcceptInvitationResponseDto(
  [property: MemberNotNullWhen(false, nameof(AcceptInvitationResponseDto.ErrorMessage))]
  bool IsSuccessful,
  string? ErrorMessage = null);

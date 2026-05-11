using System.ComponentModel.DataAnnotations;

namespace PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;

public record TenantInviteRequestDto(
  [EmailAddress]
  string InviteeEmail);
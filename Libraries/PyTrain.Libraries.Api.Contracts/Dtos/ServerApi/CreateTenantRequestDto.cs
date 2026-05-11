using System.ComponentModel.DataAnnotations;

namespace PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;

public record CreateTenantRequestDto(
  [Required, StringLength(100, MinimumLength = 1)]
  string TenantName,
  [Required, EmailAddress]
  string AdminEmail,
  [Required, StringLength(128, MinimumLength = 8)]
  string AdminPassword);

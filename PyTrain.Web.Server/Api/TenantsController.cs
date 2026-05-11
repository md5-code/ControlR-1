using PyTrain.Libraries.Api.Contracts.Constants;
using PyTrain.Libraries.Api.Contracts.Dtos.ServerApi;
using PyTrain.Web.Server.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace PyTrain.Web.Server.Api;

[Route(HttpConstants.TenantsEndpoint)]
[ApiController]
[Authorize(Roles = RoleNames.ServerAdministrator)]
public class TenantsController(
  AppDb appDb,
  IUserCreator userCreator,
  ILogger<TenantsController> logger) : ControllerBase
{
  private readonly AppDb _appDb = appDb;
  private readonly IUserCreator _userCreator = userCreator;
  private readonly ILogger<TenantsController> _logger = logger;

  [HttpGet]
  public async Task<ActionResult<IReadOnlyList<TenantResponseDto>>> GetAll()
  {
    var tenants = await _appDb.Tenants
      .Select(t => new TenantResponseDto(
        t.Id,
        t.Name ?? string.Empty,
        t.Users == null ? 0 : t.Users.Count,
        t.Devices == null ? 0 : t.Devices.Count,
        t.CreatedAt))
      .OrderByDescending(t => t.CreatedAt)
      .ToListAsync();

    return tenants;
  }

  [HttpPost]
  public async Task<ActionResult<TenantResponseDto>> Create([FromBody] CreateTenantRequestDto request)
  {
    if (!ModelState.IsValid)
    {
      return ValidationProblem(ModelState);
    }

    var existing = await _appDb.Users.AnyAsync(u => u.NormalizedEmail == request.AdminEmail.ToUpperInvariant());
    if (existing)
    {
      return Conflict(new { error = "A user with that email address already exists." });
    }

    var result = await _userCreator.CreateTenantWithAdmin(
      request.TenantName.Trim(),
      request.AdminEmail.Trim(),
      request.AdminPassword);

    if (!result.Succeeded || result.User is null)
    {
      var errors = result.IdentityResult?.Errors.Select(e => e.Description).ToArray() ?? ["Unknown error."];
      _logger.LogWarning("Tenant creation failed: {Errors}", string.Join("; ", errors));
      return BadRequest(new { errors });
    }

    var tenantId = result.User.TenantId;
    var tenant = await _appDb.Tenants
      .Where(t => t.Id == tenantId)
      .Select(t => new TenantResponseDto(
        t.Id,
        t.Name ?? string.Empty,
        t.Users == null ? 0 : t.Users.Count,
        t.Devices == null ? 0 : t.Devices.Count,
        t.CreatedAt))
      .FirstOrDefaultAsync();

    if (tenant is null)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, "Tenant was created but could not be reloaded.");
    }

    _logger.LogInformation("Server admin created new tenant {TenantId} ({TenantName}) with admin {AdminEmail}.",
      tenant.Id, tenant.Name, request.AdminEmail);

    return tenant;
  }
}

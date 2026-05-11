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
    var rows = await _appDb.Tenants
      .IgnoreQueryFilters()
      .OrderByDescending(t => t.CreatedAt)
      .Select(t => new
      {
        t.Id,
        t.Name,
        t.CreatedAt,
        UserCount = _appDb.Users.IgnoreQueryFilters().Count(u => u.TenantId == t.Id),
        DeviceCount = _appDb.Devices.IgnoreQueryFilters().Count(d => d.TenantId == t.Id)
      })
      .ToListAsync();

    return rows
      .Select(r => new TenantResponseDto(r.Id, r.Name ?? string.Empty, r.UserCount, r.DeviceCount, r.CreatedAt))
      .ToList();
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
    var row = await _appDb.Tenants
      .IgnoreQueryFilters()
      .Where(t => t.Id == tenantId)
      .Select(t => new
      {
        t.Id,
        t.Name,
        t.CreatedAt,
        UserCount = _appDb.Users.IgnoreQueryFilters().Count(u => u.TenantId == t.Id),
        DeviceCount = _appDb.Devices.IgnoreQueryFilters().Count(d => d.TenantId == t.Id)
      })
      .FirstOrDefaultAsync();

    if (row is null)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, "Tenant was created but could not be reloaded.");
    }

    var tenant = new TenantResponseDto(row.Id, row.Name ?? string.Empty, row.UserCount, row.DeviceCount, row.CreatedAt);

    _logger.LogInformation("Server admin created new tenant {TenantId} ({TenantName}) with admin {AdminEmail}.",
      tenant.Id, tenant.Name, request.AdminEmail);

    return tenant;
  }
}

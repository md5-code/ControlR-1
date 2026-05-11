using PyTrain.Web.Server.Services.DeviceManagement;

namespace PyTrain.Web.Server.Extensions;

public static class DeviceAccessQueryExtensions
{
  public static IQueryable<Device> ApplyAccessScope(
    this IQueryable<Device> query,
    Guid tenantId,
    DeviceAccessScope accessScope)
  {
    if (accessScope.Kind == DeviceAccessScopeKind.ServerAdminGlobal)
    {
      return query;
    }

    query = query.Where(x => x.TenantId == tenantId);

    return accessScope.Kind switch
    {
      DeviceAccessScopeKind.TenantWide => query,
      DeviceAccessScopeKind.SingleDevice => query.Where(x => x.Id == accessScope.DeviceId),
      DeviceAccessScopeKind.TaggedDevices => query.Where(x => x.Tags!.Any(tag => accessScope.TagIds.Contains(tag.Id))),
      _ => query.Take(0)
    };
  }
}
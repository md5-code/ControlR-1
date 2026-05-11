using PyTrain.Agent.Shared.Options;
using PyTrain.Agent.Shared.Services;

namespace PyTrain.Agent.LoadTester;

internal class FakeSettingsProvider(Guid deviceId, Uri serverUri) : IOptionsAccessor
{
  public Guid DeviceId => deviceId;

  public bool DisableAutoUpdate => true;
  public int HubDtoChunkSize => 100;
  public string InstanceId { get; } = string.Empty;

  public Uri ServerUri { get; } = serverUri;

  public Guid TenantId { get; } = Guid.NewGuid();

  public string GetAppSettingsPath()
  {
    return string.Empty;
  }

  public Guid GetRequiredTenantId()
  {
    return TenantId;
  }

  public Task UpdateAppOptions(AgentAppOptions options)
  {
    return Task.CompletedTask;
  }

  public Task UpdateId(Guid uid)
  {
    return Task.CompletedTask;
  }
}

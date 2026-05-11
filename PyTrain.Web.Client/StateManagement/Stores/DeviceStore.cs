using Microsoft.AspNetCore.SignalR.Client;

namespace PyTrain.Web.Client.StateManagement.Stores;

public interface IDeviceStore : IStoreBase<DeviceResponseDto>
{ }

internal class DeviceStore : StoreBase<DeviceResponseDto>, IDeviceStore
{
  public DeviceStore(
    IPyTrainApi pytrainApi,
    ISnackbar snackbar,
    IMessenger messenger,
    ILogger<DeviceStore> logger)
    : base(pytrainApi, snackbar, logger)
  {
    messenger.Register<HubConnectionStateChangedMessage>(this, HandleHubConnectionStateChanged);
  }

  protected override Guid GetItemId(DeviceResponseDto dto)
  {
    return dto.Id;
  }

  protected override IEnumerable<DeviceResponseDto> OrderItems(IEnumerable<DeviceResponseDto> items)
  {
    return items.OrderBy(d => d.Name);
  }

  protected override async Task RefreshImpl()
  {
    var devices = new List<DeviceResponseDto>();
    await foreach (var device in PyTrainApi.Devices.GetAllDevices())
    {
      devices.Add(device);
    }
    SetItems(devices);
  }

  private async Task HandleHubConnectionStateChanged(object subscriber, HubConnectionStateChangedMessage message)
  {
    if (message.NewState == HubConnectionState.Connected)
    {
      await Refresh();
    }
  }
}
namespace PyTrain.Agent.Shared.Interfaces;

public interface IDeviceInfoProvider
{
  Task<DeviceUpdateRequestDto> GetDeviceInfo();
}

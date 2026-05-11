using PyTrain.DesktopClient.Common.Models;
using PyTrain.DesktopClient.Common.ServiceInterfaces;
using PyTrain.DesktopClient.Common.Services;
using PyTrain.Libraries.Api.Contracts.Dtos.RemoteControlDtos;
using PyTrain.Libraries.Shared.Primitives;
using System.Runtime.CompilerServices;

namespace PyTrain.DesktopClient.Linux.Tests;

internal class FakeDesktopCapturerFactory : IDesktopCapturerFactory
{
  public IDesktopCapturer CreateNew() => new FakeDesktopCapturer();
  public IDesktopCapturer GetOrCreate() => new FakeDesktopCapturer();

  private class FakeDesktopCapturer : IDesktopCapturer
  {
    public Task ChangeDisplays(string displayId) => Task.CompletedTask;
    public ValueTask DisposeAsync() => default;
    public string GetCaptureMode() => string.Empty;
    public async IAsyncEnumerable<DtoWrapper> GetCaptureStream([EnumeratorCancellation] System.Threading.CancellationToken cancellationToken) { yield break; }
    public double GetCurrentFps(System.TimeSpan window) => 0;
    public int GetCurrentQuality() => 0;
    public Task RequestKeyFrame() => Task.CompletedTask;
    public Task StartCapturingChanges(System.Threading.CancellationToken cancellationToken) => Task.CompletedTask;
    public Task<Result<PyTrain.DesktopClient.Common.Models.DisplayInfo>> TryGetSelectedDisplay() => Task.FromResult(Result.Fail<DisplayInfo>("No display"));
  }
}

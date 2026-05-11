using Tmds.DBus;

namespace PyTrain.DesktopClient.Linux.XdgPortal;

[DBusInterface("org.freedesktop.portal.Request")]
public interface IRequest : IDBusObject
{
  Task<IDisposable> WatchResponseAsync(Action<(uint response, IDictionary<string, object> results)> handler, Action<Exception>? onError = null);
}

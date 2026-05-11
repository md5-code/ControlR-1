using Bitbound.SimpleMessenger;
using PyTrain.DesktopClient.Common.Options;
using PyTrain.DesktopClient.Common.ServiceInterfaces;
using PyTrain.DesktopClient.Common.Services;
using PyTrain.DesktopClient.Services;
using PyTrain.Libraries.Ipc;
using PyTrain.Libraries.Shared.Services;
using PyTrain.Libraries.Shared.Services.FileSystem;
using PyTrain.Libraries.Shared.Services.Processes;

namespace PyTrain.DesktopClient.Startup;

internal static class ServiceCollectionExtensions
{
  public static IServiceCollection AddDesktopShellServices(
    this IServiceCollection services,
    string? instanceId)
  {
    services.AddOptions();

    services.Configure<DesktopClientOptions>(options =>
    {
      options.InstanceId = instanceId;
    });

    services
      .AddPyTrainIpcClient<DesktopClientRpcService>()
      .AddSingleton(TimeProvider.System)
      .AddSingleton<IMessenger>(new WeakReferenceMessenger())
      .AddSingleton<IProcessManager, ProcessManager>()
      .AddSingleton<IFileSystem, FileSystem>()
      .AddSingleton<ISystemEnvironment, SystemEnvironment>()
      .AddSingleton<INavigationProvider, NavigationProvider>()
      .AddSingleton<IMainWindowProvider, MainWindowProvider>()
      .AddSingleton<IThemeProvider, ThemeProvider>()
      .AddSingleton<IAppViewModel, AppViewModel>()
      .AddSingleton<IMainWindowViewModel, MainWindowViewModel>()
      .AddSingleton<IConnectionsViewModel, ConnectionsViewModel>()
      .AddSingleton<ISettingsViewModel, SettingsViewModel>()
      .AddSingleton<IAboutViewModel, AboutViewModel>()
      .AddSingleton<IRemoteControlHostManager, RemoteControlHostManager>()
      .AddSingleton<IDialogProvider, DialogProvider>()
      .AddSingleton<IUserInteractionService, UserInteractionService>()
      .AddSingleton<IDesktopClientPermissionService, DesktopClientPermissionService>()
      .AddSingleton<IDesktopPreviewProvider, DesktopPreviewProvider>()
      .AddSingleton<IChatSessionManager, ChatSessionManager>()
      .AddSingleton<IpcClientManager>()
      .AddSingleton<IIpcClientAccessor>(sp => sp.GetRequiredService<IpcClientManager>())
      .AddSingleton<IToaster, Toaster>()
      .AddSingleton<IUiThread, UiThread>()
      .AddSingleton<IImageUtility, ImageUtility>()
      .AddSingleton<IAppLifetimeNotifier, AppLifetimeNotifier>()
      .AddSingleton<IViewModelFactory, ViewModelFactory>()
      .AddSingleton<IUrlLauncher, UrlLauncher>()
      .AddSingleton<IWaiter, Waiter>()
      .AddSingleton<INavigationItemProvider, ShellNavigationItemProvider>()
      .AddTransient<MainWindow>()
      .AddTransient<ConnectionsView>()
      .AddTransient<SettingsView>()
      .AddTransient<AboutView>()
      .AddTransient<IMessageBoxViewModel, MessageBoxViewModel>()
      .AddTransient<ChatWindow>()
      .AddTransient<IChatWindowViewModel, ChatWindowViewModel>()
      .AddTransient<ToastWindow>()
      .AddTransient<IToastWindowViewModel, ToastWindowViewModel>()
      .AddHostedService(sp => sp.GetRequiredService<IpcClientManager>())
      .AddHostedService(sp => sp.GetRequiredService<IAppLifetimeNotifier>());

    return services;
  }
}

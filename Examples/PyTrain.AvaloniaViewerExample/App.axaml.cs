using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using PyTrain.AvaloniaViewerExample.ViewModels;
using PyTrain.AvaloniaViewerExample.ViewModels.Fakes;
using PyTrain.AvaloniaViewerExample.Views;
using PyTrain.Libraries.Viewer.Common.Options;
using Microsoft.Extensions.Configuration;

namespace PyTrain.AvaloniaViewerExample;

public partial class App : Application
{
  public override void Initialize()
  {
    AvaloniaXamlLoader.Load(this);
  }

  public override void OnFrameworkInitializationCompleted()
  {
    RequestedThemeVariant = ThemeVariant.Dark;

    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
    {
      DisableAvaloniaDataAnnotationValidation();

      IMainWindowViewModel viewModel = Design.IsDesignMode
        ? GetFakeViewModel()
        : GetActualViewModel();

      desktop.MainWindow = new MainWindow()
      {
        // This is just an example.
        DataContext = viewModel
      };
    }

    base.OnFrameworkInitializationCompleted();
  }

  private void DisableAvaloniaDataAnnotationValidation()
  {
    var dataValidationPluginsToRemove =
        BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

    foreach (var plugin in dataValidationPluginsToRemove)
    {
      BindingPlugins.DataValidators.Remove(plugin);
    }
  }

  private MainWindowViewModel GetActualViewModel()
  {
    var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
    // Load configuration from user secrets and appsettings.json
    var configuration = new ConfigurationBuilder()
      .AddJsonFile("appsettings.json", optional: true)
      .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
      .AddUserSecrets<App>(optional: true)
      .Build();

    // Get viewer options from configuration.
    // This is just an example. You can use whatever method you prefer  (e.g. DI with IOptions pattern) 
    // for getting these options into the PyTrainViewer component.
    var viewerOptions = new PyTrainViewerOptions
    {
      BaseUrl = Uri.TryCreate(configuration["PyTrainViewerOptions:BaseUrl"], UriKind.Absolute, out var baseUrl)
        ? baseUrl
        : throw new InvalidOperationException("PyTrainViewerOptions:BaseUrl not configured. Use: dotnet user-secrets set \"PyTrainViewerOptions:BaseUrl\" \"https://pytrain.example.com\""),
      DeviceId = Guid.Parse(configuration["PyTrainViewerOptions:DeviceId"]
        ?? throw new InvalidOperationException("PyTrainViewerOptions:DeviceId not configured. Use: dotnet user-secrets set \"PyTrainViewerOptions:DeviceId\" \"your-device-guid\"")),
      PersonalAccessToken = configuration["PyTrainViewerOptions:PersonalAccessToken"]
        ?? throw new InvalidOperationException("PyTrainViewerOptions:PersonalAccessToken not configured. Use: dotnet user-secrets set \"PyTrainViewerOptions:PersonalAccessToken\" \"your-pat-token\"")
    };

    return new MainWindowViewModel(viewerOptions);
  }

  private MainWindowViewModelFake GetFakeViewModel()
  {
    return new MainWindowViewModelFake();
  }
}

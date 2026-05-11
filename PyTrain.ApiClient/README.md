# PyTrain API Client

A .NET client library for interacting with the PyTrain API. This library provides a strongly-typed interface for making API calls to the backend of a PyTrain server.

## Features

- Strongly-typed API client generated from OpenAPI specification
- Built-in support for dependency injection
- Static builder pattern for scenarios where dependency injection is not available
- Efficient HTTP connection management via `IHttpClientFactory`
- Automatic request/response serialization

## Installation

```bash
dotnet add package PyTrain.ApiClient
```

## Quick Start

The library supports two usage patterns: dependency injection (recommended for most applications) and a static builder pattern (useful for scripts or simple scenarios).

### Option 1: Dependency Injection

#### Service Registration

Configure the client in your `Program.cs` or startup file:

```csharp
using PyTrain.ApiClient;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddPyTrainApiClient(options =>
{
    options.BaseUrl = new Uri("https://your-pytrain-server.com");
    options.PersonalAccessToken = "your-personal-access-token";
});
```

You can also load options from configuration using the overload that accepts `IConfiguration`:

```csharp
using PyTrain.ApiClient;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddPyTrainApiClient(
    builder.Configuration,
    PyTrainApiClientOptions.SectionKey);
```

With the following configuration in `appsettings.json`:

```json
{
  "PyTrainApiClient": {
    "BaseUrl": "https://your-pytrain-server.com",
    "PersonalAccessToken": "your-personal-access-token"
  }
}
```

#### Using the Client

Inject `IPyTrainApi` directly into your services:

```csharp
using PyTrain.ApiClient;

public class MyService
{
    private readonly IPyTrainApi _client;
    private readonly ILogger<MyService> _logger;

    public MyService(IPyTrainApi client, ILogger<MyService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task GetDevicesAsync(CancellationToken cancellationToken)
    {
        var devices = new List<DeviceResponseDto>();
        await foreach (var device in _client.Devices.GetAllDevices(cancellationToken).WithCancellation(cancellationToken))
        {
            devices.Add(device);
        }

        _logger.LogInformation("Retrieved {DeviceCount} devices.", devices.Count);
        foreach (var device in devices)
        {
            _logger.LogInformation(" - {DeviceName} (ID: {DeviceId})", device.Name, device.Id);
        }
    }
}
```

### Option 2: Static Builder

The static builder pattern is useful for console applications, scripts, or scenarios where you prefer not to use dependency injection.

#### Initialization

Initialize the builder once at application startup:

```csharp
using PyTrain.ApiClient;

PyTrainApiClientBuilder.Initialize(options =>
{
    options.BaseUrl = new Uri("https://your-pytrain-server.com");
    options.PersonalAccessToken = "your-personal-access-token";
});
```

#### Using the Client

Get a client instance whenever needed:

```csharp
var client = PyTrainApiClientBuilder.GetClient();
var devices = new List<DeviceResponseDto>();
await foreach (var device in client.Devices.GetAllDevices(cancellationToken).WithCancellation(cancellationToken))
{
    devices.Add(device);
}

Console.WriteLine($"Retrieved {devices.Count} devices.");
foreach (var device in devices)
{
    Console.WriteLine($" - {device.Name} (ID: {device.Id})");
}
```

## Configuration

### PyTrainApiClientOptions

| Property                     | Type     | Required | Description                                       |
|------------------------------|----------|----------|---------------------------------------------------|
| `BaseUrl`                   | `Uri`    | Yes      | The base URL of your PyTrain server             |
| `PersonalAccessToken`       | `string` | Yes      | Your personal access token for authentication     |

### Obtaining a Personal Access Token

1. Log in to your PyTrain server
2. Navigate to your account settings
3. Generate a new Personal Access Token
4. Store it securely (e.g., using User Secrets in development or secure configuration in production)

## How It Works

### IHttpClientFactory Integration

The PyTrain API Client uses `IHttpClientFactory` under the hood to manage `HttpClient` instances. This provides several benefits:

- **Prevents socket exhaustion**: Automatically manages the lifecycle of HTTP connections
- **Handles DNS changes**: Respects DNS TTL by periodically recycling connections
- **Efficient resource usage**: Pools and reuses connections
- **Handler pipeline**: Supports middleware-style message handlers for cross-cutting concerns

This means you can safely create multiple client instances without worrying about common pitfalls associated with direct `HttpClient` usage.

## Example Project

For a complete working example, see the [PyTrain.ApiClientExample](../Examples/PyTrain.ApiClientExample) project in this repository.

## License

This project is licensed under the [MIT License](../LICENSE.txt).
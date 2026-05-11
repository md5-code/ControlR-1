using PyTrain.ApiClient;
using PyTrain.ApiClientExample;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.Configure<PyTrainApiClientOptions>(
    builder.Configuration.GetSection(PyTrainApiClientOptions.SectionKey));

builder.AddPyTrainApiClient(PyTrainApiClientOptions.SectionKey);

builder.Services.AddHostedService<ExampleService>();

var app = builder.Build();
await app.RunAsync();
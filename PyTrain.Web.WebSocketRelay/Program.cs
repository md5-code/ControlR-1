using PyTrain.Libraries.WebSocketRelay.Common.Extensions;
using PyTrain.Web.WebSocketRelay.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Logging.AddSimpleConsole(options =>
{
  options.IncludeScopes = true;
  options.TimestampFormat = "yyyy-MM-dd HH:mm:ss.fff zzz ";
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
  options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services.AddWebSocketRelay();

builder.Services.AddHealthChecks();

var app = builder.Build();
app.MapHealthChecks("/health");

app.MapWebSocketRelay("/relay");

app.Run();

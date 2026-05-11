using PyTrain.Web.ServiceDefaults;

var builder = DistributedApplication.CreateBuilder(args);

var pgUser = builder.AddParameter("PgUser", true);
var pgPassword = builder.AddParameter("PgPassword", true);

var postgres = builder
    .AddPostgres(ServiceNames.Postgres, pgUser, pgPassword, port: 5432)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume("pytrain-data")
    .ExcludeFromManifest();

var pgHost = postgres.GetEndpoint("tcp");

var web = builder
    .AddProject<Projects.PyTrain_Web_Server>(ServiceNames.PyTrain, launchProfileName: "https")
    .WithEnvironment("POSTGRES_USER", pgUser)
    .WithEnvironment("POSTGRES_PASSWORD", pgPassword)
    .WithEnvironment("PyTrain_POSTGRES_HOST", pgHost)
    .WithReference(postgres)
    .WaitFor(postgres);

var webEndpoint = web.GetEndpoint("http");

builder
  .AddProject<Projects.PyTrain_Agent>(ServiceNames.PyTrainAgent, "Run")
  .WithEnvironment("AppOptions__ServerUri", webEndpoint)
  .WaitFor(web)
  .ExcludeFromManifest();

builder.Build().Run();

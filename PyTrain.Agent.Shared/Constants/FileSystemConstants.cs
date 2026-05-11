using System.Collections.Immutable;

namespace PyTrain.Agent.Shared.Constants;

public static class FileSystemConstants
{
  public static ImmutableList<string> ExcludedDrivePrefixes { get; } =
  [
    "/System/Volumes",
    "/snap",
    "/boot",
    "/var/lib/docker"
  ];  
}
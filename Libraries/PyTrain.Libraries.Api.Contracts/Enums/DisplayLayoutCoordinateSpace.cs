using System.Text.Json.Serialization;

namespace PyTrain.Libraries.Api.Contracts.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DisplayLayoutCoordinateSpace
{
  Logical,
  Physical,
}
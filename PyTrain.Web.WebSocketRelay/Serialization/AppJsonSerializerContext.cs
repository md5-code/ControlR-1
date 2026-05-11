using PyTrain.Web.WebSocketRelay.Dtos;
using System.Text.Json.Serialization;

namespace PyTrain.Web.WebSocketRelay.Serialization;

[JsonSerializable(typeof(StatusOkDto))]
public partial class AppJsonSerializerContext : JsonSerializerContext
{

}

using System.Runtime.Serialization;

namespace PyTrain.Libraries.Api.Contracts.Enums;

public enum ConnectionType
{
  [EnumMember]
  Unknown,
  [EnumMember]
  Viewer,
  [EnumMember]
  Agent,
  [EnumMember]
  Desktop
}

using System.Runtime.Serialization;

namespace PyTrain.Libraries.Api.Contracts.Enums;

public enum PowerStateChangeType
{
  [EnumMember]
  None,

  [EnumMember]
  Restart,

  [EnumMember]
  Shutdown
}

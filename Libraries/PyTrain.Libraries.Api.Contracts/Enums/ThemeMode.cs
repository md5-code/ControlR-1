using System.Runtime.Serialization;

namespace PyTrain.Libraries.Api.Contracts.Enums;

public enum ThemeMode
{
  [EnumMember]
  Auto = 0,
  [EnumMember]
  Light = 1,
  [EnumMember]
  Dark = 2
}
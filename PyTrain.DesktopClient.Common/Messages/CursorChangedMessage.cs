using PyTrain.Libraries.Api.Contracts.Enums;

namespace PyTrain.DesktopClient.Common.Messages;

public record CursorChangedMessage(
  PointerCursor Cursor,
  string? CustomCursorBase64Png = null,
  ushort XHotspot = 0,
  ushort YHotspot = 0);
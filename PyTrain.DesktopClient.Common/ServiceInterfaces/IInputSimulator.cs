using PyTrain.DesktopClient.Common.Models;
using PyTrain.Libraries.Api.Contracts.Dtos.RemoteControlDtos;
using PyTrain.Libraries.Api.Contracts.Enums;

namespace PyTrain.DesktopClient.Common.ServiceInterfaces;
public interface IInputSimulator
{
  Task InvokeKeyEvent(
    string key,
    string code,
    bool isPressed,
    KeyboardInputMode inputMode,
    KeyEventModifiersDto modifiers);
  Task InvokeMouseButtonEvent(PointerCoordinates coordinates, int button, bool isPressed);
  Task MovePointer(PointerCoordinates coordinates, MovePointerType moveType);
  Task ResetKeyboardState();
  Task ScrollWheel(PointerCoordinates coordinates, int scrollY, int scrollX);
  Task<bool> SetBlockInput(bool isBlocked);
  Task TypeText(string text);
}

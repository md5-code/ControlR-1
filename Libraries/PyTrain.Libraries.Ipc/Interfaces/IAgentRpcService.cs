using PyTrain.Libraries.Api.Contracts.Dtos.IpcDtos;
using PolyType;
using StreamJsonRpc;

namespace PyTrain.Libraries.Ipc.Interfaces;

[JsonRpcContract]
[GenerateShape(IncludeMethods = MethodShapeFlags.PublicInstance)]
public partial interface IAgentRpcService
{
    Task<bool> SendChatResponse(ChatResponseIpcDto dto);
}

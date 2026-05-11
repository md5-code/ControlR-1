using System.Buffers;
using PyTrain.Libraries.Api.Contracts.Enums;
using PyTrain.Libraries.Api.Contracts.Dtos.RemoteControlDtos;
using SkiaSharp;

namespace PyTrain.DesktopClient.Common.Services.Encoders;

public interface IFrameEncoder
{
    CaptureEncoderType Type { get; }

    byte[] EncodeFullFrame(SKBitmap frame, int quality, ImageFormat format = ImageFormat.Jpeg);
    byte[] EncodeRegion(SKBitmap frame, SKRect region, int quality, ImageFormat format = ImageFormat.Jpeg);
}

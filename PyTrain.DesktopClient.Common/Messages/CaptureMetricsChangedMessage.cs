using PyTrain.Libraries.Api.Contracts.Dtos.RemoteControlDtos;

namespace PyTrain.DesktopClient.Common.Messages;
public record CaptureMetricsChangedMessage(CaptureMetricsDto MetricsDto);
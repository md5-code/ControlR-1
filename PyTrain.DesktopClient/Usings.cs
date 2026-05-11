global using Microsoft.Extensions.DependencyInjection;
global using PyTrain.DesktopClient.Common.ServiceInterfaces;
global using PyTrain.DesktopClient.Common.ServiceInterfaces.Toaster;
global using Avalonia.Threading;
global using PyTrain.DesktopClient.Extensions;
global using PyTrain.DesktopClient.ViewModels;
global using PyTrain.DesktopClient.ViewModels.Internals;
global using PyTrain.Libraries.Shared.Extensions;
global using PyTrain.DesktopClient.Views;
global using CommunityToolkit.Mvvm.Input;
global using PyTrain.DesktopClient.Controls;
global using PyTrain.Libraries.Avalonia;
global using PyTrain.Libraries.Avalonia.Theming;
global using PyTrain.Libraries.Avalonia.Controls;
global using System.Diagnostics.CodeAnalysis;
global using CommunityToolkit.Mvvm.ComponentModel;
global using PyTrain.DesktopClient.Common;
global using PyTrain.Libraries.Api.Contracts.Enums;
global using PyTrain.DesktopClient.Common.ViewModelInterfaces;
global using PyTrain.Libraries.Shared.Collections;

#if IS_WINDOWS
global using PyTrain.DesktopClient.Windows;
global using PyTrain.DesktopClient.Windows.Services;
global using PyTrain.Libraries.NativeInterop.Windows;
#endif

#if IS_MACOS
global using PyTrain.DesktopClient.Mac;
global using PyTrain.DesktopClient.Mac.Services;
global using PyTrain.DesktopClient.ViewModels.Mac;
global using PyTrain.DesktopClient.Views.Mac;
global using PyTrain.Libraries.NativeInterop.Unix;
global using PyTrain.Libraries.NativeInterop.Mac;
global using PyTrain.DesktopClient.Mac.Helpers;
#endif

#if IS_LINUX
global using PyTrain.DesktopClient.Linux;
global using PyTrain.DesktopClient.Linux.Services;
global using PyTrain.DesktopClient.Views.Linux;
global using PyTrain.Libraries.NativeInterop.Linux;
global using PyTrain.Libraries.NativeInterop.Unix;
global using PyTrain.DesktopClient.ViewModels.Linux;
global using PyTrain.DesktopClient.Linux.XdgPortal;
#endif
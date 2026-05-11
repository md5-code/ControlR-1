global using Xunit;

#if IS_WINDOWS
global using PyTrain.DesktopClient.Windows;
#elif IS_LINUX
global using PyTrain.DesktopClient.Linux;
#elif IS_MACOS
global using PyTrain.DesktopClient.Mac;
#endif
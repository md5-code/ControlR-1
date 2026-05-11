namespace PyTrain.Agent.Shared.Interfaces;

/// <summary>
/// Provides methods to control PyTrain services on the current platform.
/// </summary>
public interface IServiceControl
{
    /// <summary>
    /// Starts the main PyTrain agent service.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task StartAgentService(bool throwOnFailure);

    /// <summary>
    /// Starts the PyTrain desktop client service.
    /// On Mac: Starts the LaunchAgent for the desktop client.
    /// On Linux: Starts the systemd user service for the desktop client.
    /// On Windows: Throws NotSupportedException as desktop client is handled differently.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="NotSupportedException">Thrown on Windows platform.</exception>
    Task StartDesktopClientService(bool throwOnFailure);

    /// <summary>
    /// Stops the main PyTrain agent service.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task StopAgentService(bool throwOnFailure);

    /// <summary>
    /// Stops the PyTrain desktop client service.
    /// On Mac: Stops the LaunchAgent for the desktop client.
    /// On Linux: Stops the systemd user service for the desktop client.
    /// On Windows: Throws NotSupportedException as desktop client is handled differently.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="NotSupportedException">Thrown on Windows platform.</exception>
    Task StopDesktopClientService(bool throwOnFailure);
}

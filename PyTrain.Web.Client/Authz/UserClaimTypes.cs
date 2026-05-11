namespace PyTrain.Web.Client.Authz;

public static class UserClaimTypes
{
  public const string AuthenticationMethod = "pytrain:auth:method";
  // New explicit claim indicating that the authenticated session is restricted to ONLY this device.
  public const string DeviceSessionScope = "pytrain:device:scope:id";
  public const string TenantId = "pytrain:tenant:id";
  public const string UserId = "pytrain:user:id";
}
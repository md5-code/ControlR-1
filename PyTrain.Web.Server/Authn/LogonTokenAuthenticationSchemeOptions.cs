using Microsoft.AspNetCore.Authentication;

namespace PyTrain.Web.Server.Authn;

public class LogonTokenAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
  public const string DefaultScheme = "LogonToken";
}

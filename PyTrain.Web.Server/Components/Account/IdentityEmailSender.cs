using Microsoft.AspNetCore.Identity.UI.Services;

namespace PyTrain.Web.Server.Components.Account;

internal sealed class IdentityEmailSender(IEmailSender emailSender) : IEmailSender<AppUser>
{
  private readonly IEmailSender _emailSender = emailSender;

  public Task SendConfirmationLinkAsync(AppUser user, string email, string confirmationLink)
  {
    return _emailSender.SendEmailAsync(
      email,
      "PyTrain Account Confirmation",
      $"Please confirm your PyTrain account by following this link: <a href='{confirmationLink}'>{confirmationLink}</a>.");
  }

  public Task SendPasswordResetCodeAsync(AppUser user, string email, string resetCode)
  {
    return _emailSender.SendEmailAsync(
      email,
      "PyTrain Password Reset",
      $"Please reset your PyTrain password using the following code: {resetCode}");
  }

  public Task SendPasswordResetLinkAsync(AppUser user, string email, string resetLink)
  {
    return _emailSender.SendEmailAsync(
      email,
      "PyTrain Password Reset",
      $"Please reset your PyTrain password by following this link: <a href='{resetLink}'>{resetLink}</a>.");
  }
}
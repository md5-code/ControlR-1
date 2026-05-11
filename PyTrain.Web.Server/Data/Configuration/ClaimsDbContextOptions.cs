namespace PyTrain.Web.Server.Data.Configuration;

public class ClaimsDbContextOptions
{
  public Guid TenantId { get; init; }
  public Guid UserId { get; init; }
}

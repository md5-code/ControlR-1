namespace PyTrain.Web.Client.StateManagement.Stores;

public interface IInviteStore : IStoreBase<TenantInviteResponseDto>
{ }

public class InviteStore(
  IPyTrainApi pytrainApi,
  ISnackbar snackbar,
  ILogger<StoreBase<TenantInviteResponseDto>> logger)
  : StoreBase<TenantInviteResponseDto>(pytrainApi, snackbar, logger), IInviteStore
{
  private readonly IPyTrainApi _pytrainApi = pytrainApi;

  protected override Guid GetItemId(TenantInviteResponseDto dto)
  {
    return dto.Id;
  }

  protected override async Task RefreshImpl()
  {
    var getResult = await _pytrainApi.Invites.GetPendingTenantInvites();
    if (!getResult.IsSuccess)
    {
      Snackbar.Add(getResult.Reason, Severity.Error);
      return;
    }

    SetItems(getResult.Value);
  }
}

namespace PyTrain.Web.Client.StateManagement.Stores;

public interface IRoleStore : IStoreBase<RoleViewModel>
{ }

internal class RoleStore(
  IPyTrainApi pytrainApi,
  ISnackbar snackbar,
  ILogger<RoleStore> logger)
  : StoreBase<RoleViewModel>(pytrainApi, snackbar, logger), IRoleStore
{
  protected override Guid GetItemId(RoleViewModel dto)
  {
    return dto.Id;
  }

  protected override async Task RefreshImpl()
  {
    var getResult = await PyTrainApi.Roles.GetAllRoles();
    if (!getResult.IsSuccess)
    {
      Snackbar.Add(getResult.Reason, Severity.Error);
      return;
    }
    
    var vms = getResult.Value.Select(role => new RoleViewModel(role));
    SetItems(vms);
  }
}
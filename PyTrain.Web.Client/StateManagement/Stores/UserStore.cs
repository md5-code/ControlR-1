namespace PyTrain.Web.Client.StateManagement.Stores;

public interface IUserStore : IStoreBase<UserResponseDto>
{
}

public class UserStore(
  IPyTrainApi pytrainApi,
  ISnackbar snackbar,
  ILogger<UserStore> logger) : StoreBase<UserResponseDto>(pytrainApi, snackbar, logger), IUserStore
{
  protected override Guid GetItemId(UserResponseDto dto)
  {
    return dto.Id;
  }

  protected override async Task RefreshImpl()
  {
    var getResult = await PyTrainApi.Users.GetAllUsers();
    if (!getResult.IsSuccess)
    {
      Snackbar.Add(getResult.Reason, Severity.Error);
      return;
    }
    SetItems(getResult.Value);
  }
}
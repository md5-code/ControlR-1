namespace PyTrain.Web.Client.StateManagement.Stores;
public interface IUserTagStore : IStoreBase<TagViewModel>
{ }

public class UserTagStore(IPyTrainApi pytrainApi, ISnackbar snackbar, ILogger<AdminTagStore> logger)
  : StoreBase<TagViewModel>(pytrainApi, snackbar, logger), IUserTagStore
{
  protected override Guid GetItemId(TagViewModel dto)
  {
    return dto.Id;
  }

  protected override IEnumerable<TagViewModel> OrderItems(IEnumerable<TagViewModel> items)
  {
    return items.OrderBy(t => t.Name);
  }

  protected override async Task RefreshImpl()
  {
    var getResult = await PyTrainApi.UserTags.GetAllowedTags();
    if (!getResult.IsSuccess)
    {
      Snackbar.Add(getResult.Reason, Severity.Error);
      return;
    }

    SetItems(getResult.Value.Select(t => new TagViewModel(t)));
  }
}
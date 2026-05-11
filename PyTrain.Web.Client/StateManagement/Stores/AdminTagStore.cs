namespace PyTrain.Web.Client.StateManagement.Stores;

public interface IAdminTagStore : IStoreBase<TagViewModel>
{ }

public class AdminTagStore(IPyTrainApi pytrainApi, ISnackbar snackbar, ILogger<AdminTagStore> logger)
  : StoreBase<TagViewModel>(pytrainApi, snackbar, logger), IAdminTagStore
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
    var getResult = await PyTrainApi.Tags.GetAllTags(includeLinkedIds: true);
    if (!getResult.IsSuccess)
    {
      Snackbar.Add(getResult.Reason, Severity.Error);
      return;
    }

    var vms = getResult.Value.Select(tag => new TagViewModel(tag));
    SetItems(vms);
  }
}
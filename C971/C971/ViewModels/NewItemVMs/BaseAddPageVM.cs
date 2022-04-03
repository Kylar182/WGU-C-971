using System.Threading.Tasks;
using C971.Models.DatabaseModels;

namespace C971.ViewModels.NewItemVMs
{
  /// <summary>
  /// Base View Model for New DB Model Pages
  /// </summary>
  /// <typeparam name="T">
  /// Database Model for CRUD
  /// </typeparam>
  public abstract class BaseAddPageVM<T> : BaseRUDPageVM<T> where T : BaseModel, new()
  {
    /// <inheritdoc cref="BaseAddPageVM{}" />
    public BaseAddPageVM()
    {
      Item = new();
    }

    /// <summary>
    /// Initial Page Load Visibility Method
    /// </summary>
    public void OnAppearing()
    {
      IsBusy = true;
    }

    protected override async Task SaveItem()
    {
      if (Valid)
        await Service.Add(Item);
      else
        return;
    }
  }
}

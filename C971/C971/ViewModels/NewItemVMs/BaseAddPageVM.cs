using System;
using System.Threading.Tasks;
using C971.Models.DatabaseModels;
using Xamarin.Forms;

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
    public BaseAddPageVM()
    {
      Item = new();
    }

    /// <inheritdoc cref="BaseAddPageVM{}" />
    public BaseAddPageVM(Func<Task> save)
    {
      Item = new();
      Save = new Command(async () => await save?.Invoke());
    }

    /// <summary>
    /// Initial Page Load Visibility Method
    /// </summary>
    public void OnAppearing()
    {
      IsBusy = true;
    }

    public override async Task SaveItem()
    {
      if (Valid)
        await Service.Add(Item);
      else
        return;
    }
  }
}

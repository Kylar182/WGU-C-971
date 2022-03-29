using System.Collections.ObjectModel;
using C971.Models.DatabaseModels;
using Xamarin.Forms;

namespace C971.ViewModels
{
  /// <summary>
  /// Base View Model for CRUD Pages
  /// </summary>
  /// <typeparam name="T">
  /// Database Model for CRUD
  /// </typeparam>
  public abstract class BaseCrudPageVM<T> : BaseViewModel where T : BaseModel
  {
    public ObservableCollection<T> Items { get; }

    public Command LoadItemsCommand { get; }
    public Command AddItemCommand { get; }
    public Command<T> ItemTapped { get; }

    protected virtual string NewPage => "New" + typeof(T).Name + "Page";
    protected virtual string UpdatePage => "Update" + typeof(T).Name + "Page";

    public void OnAppearing()
    {
      IsBusy = true;
      Selected = null;
    }

    private T _selected;
    public T Selected
    {
      get => _selected;
      set
      {
        SetProperty(ref _selected, value);
        OnSelected(value);
      }
    }

    protected async void OnAddItem(object obj)
    {
      await Shell.Current.GoToAsync(NewPage);
    }

    protected async void OnSelected(T selected)
    {
      await Shell.Current.GoToAsync($"{UpdatePage}?Id={selected.Id}");
    }
  }
}

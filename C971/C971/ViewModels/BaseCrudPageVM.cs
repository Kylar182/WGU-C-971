using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
    /// <summary>
    /// Observable Collection of this Database Model T Type
    /// </summary>
    public ObservableCollection<T> Items { get; }

    /// <summary>
    /// Command to Load / Refresh the Page's Collection Data
    /// </summary>
    public Command LoadItemsCommand { get; }
    /// <summary>
    /// Command to Navigate to the New Item Page for this T Type
    /// </summary>
    public Command AddItemCommand { get; }
    /// <summary>
    /// Command to Navigate to the Update Item Page for the selected Item
    /// </summary>
    public Command<T> ItemTapped { get; }

    protected virtual string NewPage => "New" + typeof(T).Name + "Page";
    protected virtual string UpdatePage => "Update" + typeof(T).Name + "Page";

    private T _selected;
    /// <summary>
    /// Item the User Clicked on in the Page
    /// </summary>
    public T Selected
    {
      get => _selected;
      set
      {
        SetProperty(ref _selected, value);
        OnSelected(value);
      }
    }

    /// <inheritdoc cref="BaseCrudPageVM{T}"/>
    public BaseCrudPageVM()
    {
      Items = new ObservableCollection<T>();
      LoadItemsCommand = new Command(async () => await ExecuteLoadItems());
      AddItemCommand = new Command(OnAddItem);
      ItemTapped = new Command<T>(OnSelected);
    }

    /// <summary>
    /// Initial Page Load Visibility Method
    /// </summary>
    public void OnAppearing()
    {
      IsBusy = true;
      Selected = null;
    }

    /// <summary>
    /// Task overridden in Inherited VMs that Loads the Page's <para />
    /// Items and adds them to the observable Collection
    /// </summary>
    protected virtual Task ExecuteLoadItems()
    {
      return Task.CompletedTask;
    }

    /// <summary>
    /// Task to Navigate to the New Item Page for this T Type
    /// </summary>
    protected async void OnAddItem(object obj)
    {
      await Shell.Current.GoToAsync(NewPage);
    }

    /// <summary>
    /// Task to Navigate to the Update Item Page for the selected Item
    /// </summary>
    /// <param name="selected">
    /// Item the User Clicked on in the Page
    /// </param>
    protected async void OnSelected(T selected)
    {
      if (selected == null)
        return;

      await Shell.Current.GoToAsync($"{UpdatePage}?Id={selected.Id}");
    }
  }
}

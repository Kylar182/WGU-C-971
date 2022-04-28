using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using C971.Extensions;
using C971.Models.DatabaseModels;
using C971.Services;
using Xamarin.Forms;

namespace C971.ViewModels
{
  /// <summary>
  /// Base View Model for CRUD Pages
  /// </summary>
  /// <typeparam name="T">
  /// Database Model for CRUD
  /// </typeparam>
  public abstract class BaseCRUDPageVM<T> : BaseViewModel where T : BaseModel
  {
    /// <summary>
    /// DB CRUD Service for this Database Model
    /// </summary>
    protected ICRUDService<T> Service;

    /// <summary>
    /// Observable Collection of this Database Model T Type
    /// </summary>
    public ObservableCollection<T> Items { get; }

    /// <summary>
    /// Command to Load / Refresh the Page's Collection Data
    /// </summary>
    public ICommand LoadItemsCommand { get; }
    /// <summary>
    /// Command to Navigate to the New Item Page for this T Type
    /// </summary>
    public ICommand AddItemCommand { get; }
    /// <summary>
    /// Command to Navigate to the Update Item Page for the selected Item
    /// </summary>
    public Command<T> ItemTapped { get; }

    /// <summary>
    /// Page Title Base if not overwritten
    /// </summary>
    protected new string title = typeof(T).Name.ToPlural();

    protected virtual string NewPage => typeof(T).Name + "CUD" + "Page";
    protected virtual string UpdatePage => typeof(T).Name + "CUD" + "Page";

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

    /// <inheritdoc cref="BaseCRUDPageVM{T}"/>
    public BaseCRUDPageVM()
    {
      Items = new ObservableCollection<T>();
      LoadItemsCommand = new Command(async () => await ExecuteLoadItems());
      AddItemCommand = new Command(async () => await OnAddItem());
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
    protected virtual async Task ExecuteLoadItems()
    {
      IsBusy = true;

      try
      {
        Items.Clear();

        List<T> items = await Service.GetAll();

        foreach (T item in items)
          Items.Add(item);
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex);
      }
      finally
      {
        IsBusy = false;
      }
    }

    /// <summary>
    /// Task to Navigate to the New Item Page for this T Type
    /// </summary>
    protected async Task OnAddItem()
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

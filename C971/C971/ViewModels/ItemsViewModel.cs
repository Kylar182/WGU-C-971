using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using C971.Models;
using C971.Models.DatabaseModels;
using C971.Services;
using C971.Views;
using Xamarin.Forms;

namespace C971.ViewModels
{
  public class ItemsViewModel : BaseViewModel
  {
    private Item _selectedItem;
    public IAcademicTermService Service => DependencyService.Get<IAcademicTermService>();

    public ObservableCollection<AcademicTerm> Items { get; }
    public Command LoadItemsCommand { get; }
    public Command AddItemCommand { get; }
    public Command<Item> ItemTapped { get; }

    public ItemsViewModel()
    {
      Title = "Browse";
      Items = new ObservableCollection<AcademicTerm>();
      LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

      ItemTapped = new Command<Item>(OnItemSelected);

      AddItemCommand = new Command(OnAddItem);
    }

    async Task ExecuteLoadItemsCommand()
    {
      IsBusy = true;

      try
      {
        Items.Clear();
        List<AcademicTerm> items = await Service.GetAll();
        foreach (var item in items)
        {
          Items.Add(item);
        }
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

    public void OnAppearing()
    {
      IsBusy = true;
      SelectedItem = null;
    }

    public Item SelectedItem
    {
      get => _selectedItem;
      set
      {
        SetProperty(ref _selectedItem, value);
        OnItemSelected(value);
      }
    }

    private async void OnAddItem(object obj)
    {
      await Shell.Current.GoToAsync(nameof(NewItemPage));
    }

    async void OnItemSelected(Item item)
    {
      if (item == null)
        return;

      // This will push the ItemDetailPage onto the navigation stack
      await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
    }
  }
}

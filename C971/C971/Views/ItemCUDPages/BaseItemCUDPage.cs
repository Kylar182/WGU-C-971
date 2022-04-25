using System;
using System.Threading.Tasks;
using C971.Models.DatabaseModels;
using C971.ViewModels;
using Xamarin.Forms;

namespace C971.Views.ItemCUDPages
{
  public class BaseItemCUDPage<VM, T> : ContentPage where VM : BaseRUDPageVM<T> where T : BaseModel, new()
  {
    /// <summary>
    /// Add Page View Model for this Database Model T Type
    /// </summary>
    protected VM _viewModel;

    /// <inheritdoc cref="BaseRUDPageVM{T}.OnAppearing"/>
    protected override void OnAppearing()
    {
      base.OnAppearing();
      _viewModel.OnAppearing();
    }

    /// <summary>
    /// Async Delete Event for Item
    /// </summary>
    protected virtual async Task OnDeleteClicked()
    {
      try
      {
        await _viewModel.DeleteItem();
        await Shell.Current.GoToAsync("..");
      }
      catch (Exception ex)
      {
        await DisplayAlert("Error", ex.Message, "OK");
      }
    }
  }
}

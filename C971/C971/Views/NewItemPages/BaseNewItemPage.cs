using C971.Models.DatabaseModels;
using C971.ViewModels.NewItemVMs;
using Xamarin.Forms;

namespace C971.Views.NewItemPages
{
  public class BaseNewItemPage<VM, T> : ContentPage where VM : BaseAddPageVM<T> where T : BaseModel, new()
  {
    /// <summary>
    /// Add Page View Model for this Database Model T Type
    /// </summary>
    protected VM _viewModel;

    /// <inheritdoc cref="BaseAddPageVM{T}.OnAppearing"/>
    protected override void OnAppearing()
    {
      base.OnAppearing();
      _viewModel.OnAppearing();
    }
  }
}

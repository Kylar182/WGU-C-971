using C971.Models.DatabaseModels;
using C971.ViewModels;
using Xamarin.Forms;

namespace C971.Views
{
  /// <summary>
  /// Base Page for all Database CRUD Item Pages
  /// </summary>
  /// <typeparam name="VM">
  /// Crud Page View Model
  /// </typeparam>
  /// <typeparam name="T">
  /// Database Crud Model
  /// </typeparam>
  public class BaseCrudPage<VM, T> : ContentPage where VM : BaseCrudPageVM<T> where T : BaseModel
  {
    /// <summary>
    /// Crud Page View Model for this Database Model T Type
    /// </summary>
    protected VM _viewModel;

    /// <inheritdoc cref="BaseCrudPageVM{T}.OnAppearing"/>
    protected override void OnAppearing()
    {
      base.OnAppearing();
      _viewModel.OnAppearing();
    }
  }
}

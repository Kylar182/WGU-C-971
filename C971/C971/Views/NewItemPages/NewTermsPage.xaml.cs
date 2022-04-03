using System;
using System.Threading.Tasks;
using C971.Models.DatabaseModels;
using C971.ViewModels.NewItemVMs;
using SQLite;

namespace C971.Views.NewItemPages
{
  public partial class NewTermsPage : BaseNewItemPage<NewTermVM, AcademicTerm>
  {
    public NewTermsPage()
    {
      InitializeComponent();

      BindingContext = _viewModel = new NewTermVM(async () => await OnSaveButtonClicked());
    }

    /// <summary>
    /// Async Save Event for New Items
    /// </summary>
    /// <param name="sender">
    /// Object that called the Event
    /// </param>
    /// <param name="args">
    /// Event Arguments
    /// </param>
    private async Task OnSaveButtonClicked()
    {
      try
      {
        await _viewModel.SaveItem();
      }
      catch (SQLiteException ex)
      {
        if ((ex.Message).Contains("UNIQUE"))
          await DisplayAlert("Error", "Term name already exists", "OK");
        else
          await DisplayAlert("Error", ex.Message, "OK");
      }
      catch (Exception ex)
      {
        await DisplayAlert("Error", ex.Message, "OK");
      }
    }
  }
}

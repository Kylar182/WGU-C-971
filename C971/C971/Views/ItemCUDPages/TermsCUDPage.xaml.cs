using System;
using System.Threading.Tasks;
using C971.Models.DatabaseModels;
using C971.ViewModels.ItemCUDVMs;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971.Views.ItemCUDPages
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class TermsCUDPage : BaseItemCUDPage<TermCUDVM, AcademicTerm>
  {
    public TermsCUDPage()
    {
      InitializeComponent();

      BindingContext = _viewModel = new TermCUDVM(async () => await OnSaveButtonClicked());
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
        await Shell.Current.GoToAsync("..");
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

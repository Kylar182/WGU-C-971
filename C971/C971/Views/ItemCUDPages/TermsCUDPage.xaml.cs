using System;
using System.Threading.Tasks;
using C971.Extensions;
using C971.Models.DatabaseModels;
using C971.ViewModels;
using C971.ViewModels.ItemCUDVMs;
using SQLite;
using Xamarin.Forms;

namespace C971.Views.ItemCUDPages
{
  [QueryProperty(nameof(TermId), nameof(BaseRUDPageVM<AcademicTerm>.Id))]
  public partial class TermsCUDPage : BaseItemCUDPage<TermCUDVM, AcademicTerm>
  {
    public TermsCUDPage()
    {
      InitializeComponent();

      BindingContext = _viewModel = new TermCUDVM(async () => await OnSaveClicked(),
                                                                      async () => await OnDeleteClicked());
    }

    /// <summary>
    /// Course Id from Query, if Any
    /// </summary>
    public string TermId
    {
      set
      {
        LoadTerm(value).ConfigureAwait(true);
      }
    }

    /// <summary>
    /// Async Save Event for New Items
    /// </summary>
    private async Task OnSaveClicked()
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

    /// <summary>
    /// Load the Term from Navigation, if Any
    /// </summary>
    /// <param name="id">
    /// Term Id
    /// </param>
    private async Task LoadTerm(string id)
    {
      try
      {
        if (id.NotEmpty() && _viewModel != null && int.TryParse(id, out int termId))
          await _viewModel.LoadTerm(termId);
      }
      catch (SQLiteException ex)
      {
        await DisplayAlert("SQL Error", ex.Message, "OK");
      }
      catch (Exception ex)
      {
        await DisplayAlert("Error", ex.Message, "OK");
      }
    }
  }
}

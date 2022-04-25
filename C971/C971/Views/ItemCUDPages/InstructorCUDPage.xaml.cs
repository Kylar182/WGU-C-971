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
  [QueryProperty(nameof(InstructorId), nameof(BaseRUDPageVM<Instructor>.Id))]
  public partial class InstructorCUDPage : BaseItemCUDPage<InstructorCUDVM, Instructor>
  {
    public InstructorCUDPage()
    {
      InitializeComponent();

      BindingContext = _viewModel = new InstructorCUDVM(async () => await OnSaveButtonClicked());
    }

    /// <summary>
    /// Instructor Id from Query, if Any
    /// </summary>
    public string InstructorId
    {
      set
      {
        LoadInstructor(value).ConfigureAwait(true);
      }
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
          await DisplayAlert("Error", "Instructor Name already exists", "OK");
        else
          await DisplayAlert("Error", ex.Message, "OK");
      }
      catch (Exception ex)
      {
        await DisplayAlert("Error", ex.Message, "OK");
      }
    }

    /// <summary>
    /// Load the Instructor from Navigation, if Any
    /// </summary>
    /// <param name="id">
    /// Instructor Id
    /// </param>
    private async Task LoadInstructor(string id)
    {
      try
      {
        if (id.NotEmpty() && _viewModel != null && int.TryParse(id, out int InstructorId))
          await _viewModel.LoadInstructor(InstructorId);
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

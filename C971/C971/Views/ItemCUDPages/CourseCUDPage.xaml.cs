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
  [QueryProperty(nameof(CourseId), nameof(BaseRUDPageVM<Course>.Id))]
  public partial class CourseCUDPage : BaseItemCUDPage<CourseCUDVM, Course>
  {
    public CourseCUDPage()
    {
      InitializeComponent();

      BindingContext = _viewModel = new CourseCUDVM(async () => await OnSaveButtonClicked());
    }

    /// <summary>
    /// Course Id from Query, if Any
    /// </summary>
    public string CourseId
    {
      set
      {
        LoadCourse(value).ConfigureAwait(true);
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
        if (_viewModel.ObjAssessmentId != null && _viewModel.PerfAssessmentId != null)
          await Shell.Current.GoToAsync("..");
      }
      catch (SQLiteException ex)
      {
        if ((ex.Message).Contains("UNIQUE"))
          await DisplayAlert("Error", "Course name already exists", "OK");
        else
          await DisplayAlert("Error", ex.Message, "OK");
      }
      catch (Exception ex)
      {
        await DisplayAlert("Error", ex.Message, "OK");
      }
    }

    /// <summary>
    /// Load the Course from Navigation, if Any
    /// </summary>
    /// <param name="id">
    /// Course Id
    /// </param>
    private async Task LoadCourse(string id)
    {
      try
      {
        if (id.NotEmpty() && _viewModel != null && int.TryParse(id, out int courseId))
          await _viewModel.LoadCourse(courseId);
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

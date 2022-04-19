using System;
using System.Threading.Tasks;
using C971.Models.DatabaseModels;
using C971.ViewModels.NewItemVMs;
using SQLite;
using Xamarin.Forms;

namespace C971.Views.NewItemPages
{
  [QueryProperty(nameof(CourseId), nameof(CourseId))]
  public partial class NewCoursePage : BaseNewItemPage<NewCourseVM, Course>
  {
    public NewCoursePage()
    {
      InitializeComponent();

      BindingContext = _viewModel = new NewCourseVM(async () => await OnSaveButtonClicked());
    }

    /// <summary>
    /// Course Id from Query, if Any
    /// </summary>
    public int? CourseId
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
    private async Task LoadCourse(int? id)
    {
      if (_viewModel != null)
        _viewModel.IsBusy = true;
      try
      {
        if (id != null && _viewModel != null)
          await _viewModel.LoadCourseAsync(id);
        if (_viewModel != null)
          _viewModel.IsBusy = false;
      }
      catch (SQLiteException ex)
      {
        if (_viewModel != null)
          _viewModel.IsBusy = false;
        await DisplayAlert("SQL Error", ex.Message, "OK");
      }
      catch (Exception ex)
      {
        if (_viewModel != null)
          _viewModel.IsBusy = false;
        await DisplayAlert("Error", ex.Message, "OK");
      }
    }
  }
}

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
  [QueryProperty(nameof(OAPA), nameof(OAPA))]
  [QueryProperty(nameof(CourseId), nameof(CourseId))]
  [QueryProperty(nameof(AssessmentId), nameof(BaseRUDPageVM<Assessment>.Id))]
  public partial class AssessmentCUDPage : BaseItemCUDPage<AssessmentCUDVM, Assessment>
  {
    public AssessmentCUDPage()
    {
      InitializeComponent();

      BindingContext = _viewModel = new AssessmentCUDVM(async () => await OnSaveClicked(),
                                                                      async () => await OnDeleteClicked());
    }

    /// <summary>
    /// OA or PA from Query
    /// </summary>
    public string OAPA
    {
      set
      {
        if (_viewModel != null)
          _viewModel.OAPA = value;
      }
    }

    /// <summary>
    /// CourseId Id from Query, if Any
    /// </summary>
    public string CourseId
    {
      set
      {
        if (value.NotEmpty() && _viewModel != null && int.TryParse(value, out int courseId))
          _viewModel.CourseId = courseId;
      }
    }

    /// <summary>
    /// Assessment Id from Query, if Any
    /// </summary>
    public string AssessmentId
    {
      set
      {
        LoadAssessment(value).ConfigureAwait(true);
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
        await Shell.Current.GoToAsync($"..?Id={_viewModel.CourseId}");
      }
      catch (Exception ex)
      {
        await DisplayAlert("Error", ex.Message, "OK");
      }
    }

    /// <summary>
    /// Load the Assessment from Navigation, if Any
    /// </summary>
    /// <param name="id">
    /// Assessment Id
    /// </param>
    private async Task LoadAssessment(string id)
    {
      try
      {
        if (id.NotEmpty() && _viewModel != null && int.TryParse(id, out int assessmentId))
          await _viewModel.LoadAssessment(assessmentId);
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

using C971.Models.DatabaseModels;
using C971.ViewModels;

namespace C971.Views
{
  public partial class CoursesPage : BaseCrudPage<CourseVM, Course>
  {
    public CoursesPage()
    {
      InitializeComponent();

      BindingContext = _viewModel = new CourseVM();
    }
  }
}

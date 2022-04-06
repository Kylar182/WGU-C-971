using C971.Models.DatabaseModels;
using C971.ViewModels;
using Xamarin.Forms.Xaml;

namespace C971.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class CoursesPage : BaseCrudPage<CourseVM, Course>
  {
    public CoursesPage()
    {
      InitializeComponent();

      BindingContext = _viewModel = new CourseVM();
    }
  }
}

using C971.Models.DatabaseModels;
using C971.Services;
using Xamarin.Forms;

namespace C971.ViewModels
{
  public class CourseVM : BaseCrudPageVM<Course>
  {
    public CourseVM()
    {
      Title = "Courses";
      Service = DependencyService.Get<ICourseService>();
    }
  }
}

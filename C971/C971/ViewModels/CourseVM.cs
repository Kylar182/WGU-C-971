using C971.Models.DatabaseModels;
using C971.Services;
using Xamarin.Forms;

namespace C971.ViewModels
{
  public class CourseVM : BaseCRUDPageVM<Course>
  {
    public CourseVM()
    {
      Service = DependencyService.Get<ICourseService>();
    }
  }
}

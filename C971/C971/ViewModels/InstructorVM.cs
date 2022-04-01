using C971.Models.DatabaseModels;
using C971.Services;
using Xamarin.Forms;

namespace C971.ViewModels
{
  public class InstructorVM : BaseCrudPageVM<Instructor>
  {
    public InstructorVM()
    {
      Service = DependencyService.Get<IInstructorService>();
    }
  }
}

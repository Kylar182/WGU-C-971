using C971.Views.ItemCUDPages;
using Xamarin.Forms;

namespace C971
{
  public partial class AppShell : Shell
  {
    public AppShell()
    {
      InitializeComponent();
      Routing.RegisterRoute(nameof(TermsCUDPage), typeof(TermsCUDPage));
      Routing.RegisterRoute(nameof(InstructorCUDPage), typeof(InstructorCUDPage));
      Routing.RegisterRoute(nameof(CourseCUDPage), typeof(CourseCUDPage));
    }
  }
}

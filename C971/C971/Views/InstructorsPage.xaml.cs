using C971.Models.DatabaseModels;
using C971.ViewModels;
using Xamarin.Forms.Xaml;

namespace C971.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class InstructorsPage : BaseCrudPage<InstructorVM, Instructor>
  {
    public InstructorsPage()
    {
      InitializeComponent();

      BindingContext = _viewModel = new InstructorVM();
    }
  }
}

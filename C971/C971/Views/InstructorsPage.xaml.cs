using C971.Models.DatabaseModels;
using C971.ViewModels;

namespace C971.Views
{
  public partial class InstructorsPage : BaseCrudPage<InstructorVM, Instructor>
  {
    public InstructorsPage()
    {
      InitializeComponent();

      BindingContext = _viewModel = new InstructorVM();
    }
  }
}

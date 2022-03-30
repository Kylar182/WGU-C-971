using C971.Models.DatabaseModels;
using C971.ViewModels;

namespace C971.Views
{
  public partial class TermsPage : BaseCrudPage<AcademicTermVM, AcademicTerm>
  {
    public TermsPage()
    {
      InitializeComponent();

      BindingContext = _viewModel = new AcademicTermVM();
    }
  }
}

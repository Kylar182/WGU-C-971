using C971.Models.DatabaseModels;
using C971.ViewModels;
using Xamarin.Forms.Xaml;

namespace C971.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class TermsPage : BaseCRUDPage<AcademicTermVM, AcademicTerm>
  {
    public TermsPage()
    {
      InitializeComponent();

      BindingContext = _viewModel = new AcademicTermVM();
    }
  }
}

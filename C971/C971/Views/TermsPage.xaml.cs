using C971.ViewModels;
using Xamarin.Forms;

namespace C971.Views
{
  public partial class TermsPage : ContentPage
  {
    AcademicTermVM _viewModel;

    public TermsPage()
    {
      InitializeComponent();

      BindingContext = _viewModel = new AcademicTermVM();
    }
  }
}

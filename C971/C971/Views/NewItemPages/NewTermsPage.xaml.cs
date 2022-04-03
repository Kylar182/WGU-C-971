using C971.Models.DatabaseModels;
using C971.ViewModels.NewItemVMs;

namespace C971.Views.NewItemPages
{
  public partial class NewTermsPage : BaseNewItemPage<NewTermVM, AcademicTerm>
  {
    public NewTermsPage()
    {
      InitializeComponent();

      BindingContext = _viewModel = new NewTermVM();
    }
  }
}

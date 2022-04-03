using C971.Models.DatabaseModels;
using C971.Services;
using C971.Views.NewItemPages;
using Xamarin.Forms;

namespace C971.ViewModels
{
  public class AcademicTermVM : BaseCrudPageVM<AcademicTerm>
  {
    protected override string NewPage => nameof(NewTermsPage);

    public AcademicTermVM()
    {
      Title = "Terms";
      Service = DependencyService.Get<IAcademicTermService>();
    }
  }
}

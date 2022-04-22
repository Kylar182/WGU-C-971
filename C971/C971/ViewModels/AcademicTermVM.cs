using C971.Models.DatabaseModels;
using C971.Services;
using C971.Views.ItemCUDPages;
using Xamarin.Forms;

namespace C971.ViewModels
{
  public class AcademicTermVM : BaseCRUDPageVM<AcademicTerm>
  {
    protected override string NewPage => nameof(TermsCUDPage);

    public AcademicTermVM()
    {
      Title = "Terms";
      Service = DependencyService.Get<IAcademicTermService>();
    }
  }
}

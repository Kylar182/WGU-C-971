using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using C971.Models.DatabaseModels;
using C971.Services;
using Xamarin.Forms;

namespace C971.ViewModels
{
  public class AcademicTermVM : BaseCrudPageVM<AcademicTerm>
  {
    private readonly IAcademicTermService Service;

    public AcademicTermVM()
    {
      Title = "Terms";
      Service = DependencyService.Get<IAcademicTermService>();
    }

    protected override async Task ExecuteLoadItems()
    {
      IsBusy = true;

      try
      {
        Items.Clear();

        List<AcademicTerm> terms = await Service.GetAll();

        foreach (AcademicTerm term in terms)
          Items.Add(term);
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex);
      }
      finally
      {
        IsBusy = false;
      }
    }
  }
}

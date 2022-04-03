using System;
using System.Threading.Tasks;
using C971.Extensions;
using C971.Models.DatabaseModels;
using C971.Services;
using Xamarin.Forms;

namespace C971.ViewModels.NewItemVMs
{
  /// <summary>
  /// VM For a New Academic Term
  /// </summary>
  public class NewTermVM : BaseAddPageVM<AcademicTerm>
  {
    private string termTitle;
    /// <inheritdoc cref="AcademicTerm.TermTitle"/>
    public string TermTitle
    {
      get { return termTitle; }
      set
      {
        SetOrError(new() { new Tuple<bool, string>(value.NotEmpty(), "A Title is required") }, value);

        SetProperty(ref termTitle, value);
      }
    }

    private DateTime start;
    /// <inheritdoc cref="AcademicTerm.Start"/>
    public DateTime Start
    {
      get { return start; }
      set
      {
        DateTime val = new(value.Year, value.Month, value.Day,
                                                6, 0, 0, DateTimeKind.Utc);
        SetOrError(new() { new Tuple<bool, string>(true, "") }, val);

        SetProperty(ref start, value);

        End = val.AddMonths(6).AddTicks(-1);
      }
    }

    private DateTime end;
    /// <inheritdoc cref="AcademicTerm.End"/>
    public DateTime End
    {
      get { return end; }
      set
      {
        SetOrError(new() { new Tuple<bool, string>(true, "") }, value);

        SetProperty(ref end, value);
      }
    }

    /// <inheritdoc cref="NewTermVM" />
    public NewTermVM()
    {
      Title = "New Term";
      Service = DependencyService.Get<IAcademicTermService>();
      Start = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                                              6, 0, 0, DateTimeKind.Utc);
    }

    /// <inheritdoc cref="NewTermVM" />
    public NewTermVM(Func<Task> save) : base(save)
    {
      Title = "New Term";
      Service = DependencyService.Get<IAcademicTermService>();
      Start = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                                              6, 0, 0, DateTimeKind.Utc);

    }
  }
}

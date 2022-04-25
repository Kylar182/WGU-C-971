using System;
using System.Linq;
using System.Threading.Tasks;
using C971.Extensions;
using C971.Models.DatabaseModels;
using C971.Services;
using Xamarin.Forms;

namespace C971.ViewModels.ItemCUDVMs
{
  /// <summary>
  /// VM For a Academic Term Create / Update / Delete
  /// </summary>
  public class TermCUDVM : BaseRUDPageVM<AcademicTerm>
  {
    private string termTitle;
    /// <inheritdoc cref="AcademicTerm.TermTitle"/>
    public string TermTitle
    {
      get { return termTitle; }
      set
      {
        SetOrError(new() { new Tuple<bool, string>(value.NotEmpty(), "A Title is required") }, value.TrimFix());

        SetProperty(ref termTitle, value.TrimFix());
      }
    }

    /// <inheritdoc cref="AcademicTerm.TermTitle"/>
    public string TermTitleError => Errors.ContainsKey(nameof(TermTitle)) ? Errors[nameof(TermTitle)].First() : "";

    private DateTime start;
    /// <inheritdoc cref="AcademicTerm.Start"/>
    public DateTime Start
    {
      get { return start; }
      set
      {
        DateTime local = DateTime.Now;
        TimeZoneInfo timeZone = TimeZoneInfo.Local;
        TimeSpan offset = timeZone.GetUtcOffset(local);

        DateTime val = new(value.Year, value.Month, value.Day,
                                                12, 0, 0, DateTimeKind.Utc);
        val = val.AddHours(offset.Hours);
        val = val.AddMinutes(offset.Minutes);
        val = val.AddSeconds(offset.Seconds);
        SetOrError(new() { new Tuple<bool, string>(true, "") }, val);

        SetProperty(ref start, val);

        End = val.AddMonths(6).AddTicks(-1);
      }
    }

    /// <inheritdoc cref="AcademicTerm.Start"/>
    public string StartError => Errors.ContainsKey(nameof(Start)) ? Errors[nameof(Start)].First() : "";

    private DateTime end;
    /// <inheritdoc cref="AcademicTerm.End"/>
    public DateTime End
    {
      get { return end; }
      set
      {
        DateTime val = value;
        if (val.AddMonths(-6).AddTicks(1) != Item.Start)
        {
          DateTime local = DateTime.Now;
          TimeZoneInfo timeZone = TimeZoneInfo.Local;
          TimeSpan offset = timeZone.GetUtcOffset(local);

          val = new(value.Year, value.Month, value.Day,
                                                  12, 0, 0, DateTimeKind.Utc);
          val = val.AddHours(offset.Hours);
          val = val.AddMinutes(offset.Minutes);
          val = val.AddSeconds(offset.Seconds);
          val = val.AddTicks(-1);
        }

        SetOrError(new() { new Tuple<bool, string>(true, "") }, val);

        SetProperty(ref end, val);
      }
    }

    /// <inheritdoc cref="AcademicTerm.End"/>
    public string EndError => Errors.ContainsKey(nameof(End)) ? Errors[nameof(End)].First() : "";

    /// <inheritdoc cref="TermCUDVM" />
    public TermCUDVM()
    {
      Title = "New Term";
      TermTitle = null;
      Service = DependencyService.Get<IAcademicTermService>();
      Start = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                                              12, 0, 0, DateTimeKind.Utc);
    }

    /// <inheritdoc cref="TermCUDVM" />
    public TermCUDVM(Func<Task> save) : base(save)
    {
      Title = "New Term";
      TermTitle = null;
      Service = DependencyService.Get<IAcademicTermService>();
      Start = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                                              12, 0, 0, DateTimeKind.Utc);

      IsBusy = false;
    }

    public async Task LoadTerm(int id)
    {
      IsBusy = true;
      Id = id;
      AcademicTerm term = null;

      await Service.Get(pr => pr.Id == id).ContinueWith(t =>
      {
        if (t.Exception == null)
        {
          term = t.Result;
        }
      }).ConfigureAwait(true);

      if (term != null)
      {
        Item = term;
        Title = $"Term {id}";
        Id = term.Id;
        TermTitle = term.TermTitle;
        Start = term.Start;
        End = term.End;
      }
      else
      {
        Title = "New Term";
        TermTitle = null;
        Start = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                                                12, 0, 0, DateTimeKind.Utc);

      }

      IsBusy = false;
    }
  }
}

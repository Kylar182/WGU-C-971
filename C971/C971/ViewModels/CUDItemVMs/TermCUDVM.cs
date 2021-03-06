using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
  public class TermCUDVM : BaseCUDPageVM<AcademicTerm>
  {
    /// <inheritdoc cref="ICourseService"/>
    private readonly ICourseService _courseService;

    private string termTitle;
    /// <inheritdoc cref="AcademicTerm.TermTitle"/>
    public string TermTitle
    {
      get { return termTitle; }
      set
      {
        SetOrError(new()
        {
          new Tuple<bool, string>(value.NotEmpty(), "A Title is required"),
          new Tuple<bool, string>(value.NotEmpty() && value.Length <= 250, "Title Max 250 Characters")
        }, value.TrimFix());

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

        SetOrError(new() { new Tuple<bool, string>(true, "Start must be earlier than End") }, val);

        SetProperty(ref start, val);

        if (End == null || End < value)
          End = val.AddMonths(6).AddSeconds(-1);
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
        if (val.AddMonths(-6).AddSeconds(1) != Item.Start)
        {
          DateTime local = DateTime.Now;
          TimeZoneInfo timeZone = TimeZoneInfo.Local;
          TimeSpan offset = timeZone.GetUtcOffset(local);

          val = new(value.Year, value.Month, value.Day,
                                                  12, 0, 0, DateTimeKind.Utc);
          val = val.AddHours(offset.Hours);
          val = val.AddMinutes(offset.Minutes);
          val = val.AddSeconds(offset.Seconds);
          val = val.AddSeconds(-1);
        }

        SetOrError(new() { new Tuple<bool, string>(Start <= val, "Start must be earlier than End") }, Start, nameof(Start));
        SetOrError(new() { new Tuple<bool, string>(val >= Start, "End must be later than Start") }, val);

        SetProperty(ref end, val);
      }
    }

    /// <inheritdoc cref="AcademicTerm.End"/>
    public string EndError => Errors.ContainsKey(nameof(End)) ? Errors[nameof(End)].First() : "";

    /// <summary>
    /// Courses in this Term
    /// </summary>
    public ObservableCollection<Course> Courses { get; }

    /// <inheritdoc cref="TermCUDVM" />
    public TermCUDVM()
    {
      Title = "New Term";
      TermTitle = null;
      Courses = new ObservableCollection<Course>();
      Service = DependencyService.Get<IAcademicTermService>();
      _courseService = DependencyService.Get<ICourseService>();
      Start = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                                              12, 0, 0, DateTimeKind.Utc);
    }

    /// <inheritdoc cref="TermCUDVM" />
    public TermCUDVM(Func<Task> save, Func<Task> delete) : base(save, delete)
    {
      Title = "New Term";
      TermTitle = null;
      Courses = new ObservableCollection<Course>();
      Service = DependencyService.Get<IAcademicTermService>();
      _courseService = DependencyService.Get<ICourseService>();
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
          Id = t.Result.Id;
          Title = $"Term {id}";
          TermTitle = t.Result.TermTitle;
          Start = t.Result.Start;
          End = t.Result.End;
          Item = t.Result;
          term = t.Result;
        }
      }).ConfigureAwait(true);

      if (term == null)
      {
        Title = "New Term";
        TermTitle = null;
        Start = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                                                12, 0, 0, DateTimeKind.Utc);
      }
      else
      {
        await _courseService.GetByTerm(id).ContinueWith(t =>
        {
          if (t.Exception == null)
          {
            List<Course> courses = t.Result;

            foreach (Course course in courses)
              Courses.Add(course);
          }
        }).ConfigureAwait(true);
      }

      IsBusy = false;
    }
  }
}

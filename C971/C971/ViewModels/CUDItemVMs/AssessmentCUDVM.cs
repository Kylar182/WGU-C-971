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
  /// VM For a Course Assessment Create / Update / Delete
  /// </summary>
  public class AssessmentCUDVM : BaseRUDPageVM<Assessment>
  {
    private readonly ICourseService CourseService;

    private int courseId;
    /// <inheritdoc cref="Assessment.CourseId"/>
    public int CourseId
    {
      get { return courseId; }
      set
      {
        SetOrError(new() { new Tuple<bool, string>(value > 0, "Course Id Required") }, value);

        SetProperty(ref courseId, value);

        if (value > 0 && (Course == null || Course.Id == 0))
        {
          CourseService.Get(pr => pr.Id == value).ContinueWith(t =>
          {
            if (t.Exception == null)
            {
              Course = t.Result;
            }
          }).ConfigureAwait(true);
        }
      }
    }

    /// <inheritdoc cref="Models.DatabaseModels.Course"/>
    public Course Course { get; set; }

    private string oapa;
    /// <summary>
    /// Determines if this Assessment is an Objective Assessment (OA) or Performance Assessment (PA)
    /// </summary>
    public string OAPA
    {
      get { return oapa; }
      set
      {
        if (value.IsEmpty())
          AddError(nameof(OAPA), "OAPA is required");
        else
          RemoveError(nameof(OAPA), "OAPA is required");

        SetProperty(ref oapa, value.TrimFix());

        if (Title != null && value != null && !Title.Contains(value))
        {
          if (Title.Contains("New"))
            Title = $"New {value} Assessment";
          else if (Id != null && !Title.Contains("New"))
            Title = $"{value} Assessment {Id}";
        }
        else if (value != null)
        {
          if (Id == null)
            Title = $"New {value} Assessment";
          else if (Id != null)
            Title = $"{value} Assessment {Id}";
        }
      }
    }

    private string name;
    /// <inheritdoc cref="Assessment.Name"/>
    public string Name
    {
      get { return name; }
      set
      {
        SetOrError(new()
        {
          new Tuple<bool, string>(value.NotEmpty(), "A Name is required"),
          new Tuple<bool, string>(value.NotEmpty() && value.Length <= 150, "Name Max 150 Characters")
        }, value.TrimFix());

        SetProperty(ref name, value.TrimFix());
      }
    }

    /// <inheritdoc cref="Assessment.Name"/>
    public string NameError => Errors.ContainsKey(nameof(Name)) ? Errors[nameof(Name)].First() : "";

    private DateTime start;
    /// <inheritdoc cref="Assessment.Start"/>
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

        SetOrError(new() { new Tuple<bool, string>(val <= End, "End must be later than Start") }, End, nameof(End));
        SetOrError(new() { new Tuple<bool, string>(val <= End, "Start must be earlier than End") }, val);

        SetProperty(ref start, val);
      }
    }

    /// <inheritdoc cref="Assessment.Start"/>
    public string StartError => Errors.ContainsKey(nameof(Start)) ? Errors[nameof(Start)].First() : "";

    private DateTime end;
    /// <inheritdoc cref="Assessment.End"/>
    public DateTime End
    {
      get { return end; }
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

        SetOrError(new() { new Tuple<bool, string>(Start <= val, "Start must be earlier than End") }, Start, nameof(Start));
        SetOrError(new() { new Tuple<bool, string>(val >= Start, "End must be later than Start") }, val);

        SetProperty(ref end, val);
      }
    }

    /// <inheritdoc cref="Assessment.End"/>
    public string EndError => Errors.ContainsKey(nameof(End)) ? Errors[nameof(End)].First() : "";

    /// <inheritdoc cref="AssessmentCUDVM" />
    public AssessmentCUDVM()
    {
      if (Id == null)
      {
        Title = "New Assessment";
        Name = null;
        Start = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                                              12, 0, 0, DateTimeKind.Utc);
        End = new(DateTime.Now.Year, DateTime.Now.Month + 1, DateTime.Now.Day,
                                                              12, 0, 0, DateTimeKind.Utc);
      }
    }

    /// <inheritdoc cref="TermCUDVM" />
    public AssessmentCUDVM(Func<Task> save, Func<Task> delete) : base(save, delete)
    {
      if (Id == null)
      {
        Title = $"New {OAPA} Assessment";
        Name = null;
        Start = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                                              12, 0, 0, DateTimeKind.Utc);
        End = new(DateTime.Now.Year, DateTime.Now.Month + 1, DateTime.Now.Day,
                                                              12, 0, 0, DateTimeKind.Utc);
      }

      Service = DependencyService.Get<IAssessmentService>();
      CourseService = DependencyService.Get<ICourseService>();

      IsBusy = false;
    }

    public async Task LoadAssessment(int id)
    {
      IsBusy = true;
      Id = id;
      Assessment assessment = new();

      await Service.Get(pr => pr.Id == id).ContinueWith(t =>
      {
        if (t.Exception == null)
        {
          assessment = t.Result;
        }
      }).ConfigureAwait(true);

      if (assessment != null)
      {
        Item = assessment;
        Title = $"{OAPA} Assessment {id}";
        Id = assessment.Id;
        Name = assessment.Name;
        Start = assessment.Start;
        End = assessment.End;
        CourseId = assessment.CourseId;
      }
      else
      {
        Title = $"New {OAPA} Assessment";
        Name = null;
        Start = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                                              12, 0, 0, DateTimeKind.Utc);
        End = new(DateTime.Now.Year, DateTime.Now.Month + 1, DateTime.Now.Day,
                                                              12, 0, 0, DateTimeKind.Utc);
      }

      IsBusy = false;
    }

    public override async Task SaveItem()
    {
      await base.SaveItem();

      if (OAPA == "Objective")
        Course.ObjAssessmentId = Id;
      else
        Course.PerfAssessmentId = Id;

      await CourseService.Update(Course);
    }
  }
}

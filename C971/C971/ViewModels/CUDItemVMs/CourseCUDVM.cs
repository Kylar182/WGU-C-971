using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using C971.Extensions;
using C971.Models.DatabaseModels;
using C971.Models.Enums;
using C971.Services;
using C971.Views.ItemCUDPages;
using Xamarin.Forms;

namespace C971.ViewModels.ItemCUDVMs
{
  /// <summary>
  /// VM For a Academic Course Create / Update / Delete
  /// </summary>
  public class CourseCUDVM : BaseCUDPageVM<Course>
  {
    /// <inheritdoc cref="IAcademicTermService"/>
    private readonly IAcademicTermService _termService;
    /// <inheritdoc cref="IInstructorService"/>
    private readonly IInstructorService _instructorService;
    /// <inheritdoc cref="IAssessmentService"/>
    private readonly IAssessmentService _assessmentService;

    private string name;
    /// <inheritdoc cref="Course.Name"/>
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

    /// <inheritdoc cref="Course.Name"/>
    public string NameError => Errors.ContainsKey(nameof(Name)) ? Errors[nameof(Name)].First() : "";

    /// <summary>
    /// Command to Navigate to the Assessment Create / Update / Delete <para />
    /// Page for the Performance Assessment related to this Course (if Any)
    /// </summary>
    public ICommand PACommand { get; }

    /// <summary>
    /// Command to Navigate to the Assessment Create / Update / Delete <para />
    /// Page for the Objective Assessment related to this Course (if Any)
    /// </summary>
    public ICommand OACommand { get; }

    /// <summary>
    /// Command to Share Notes
    /// </summary>
    public ICommand ShareCommand { get; }

    private string description;
    /// <inheritdoc cref="Course.Description"/>
    public string Description
    {
      get { return description; }
      set
      {
        SetOrError(new()
        {
          new Tuple<bool, string>(value.NotEmpty(), "A Description is required"),
          new Tuple<bool, string>(value.NotEmpty() && value.Length <= 1500, "Description Max 1000 Characters")
        }, value.TrimFix());

        SetProperty(ref description, value.TrimFix());
      }
    }

    /// <inheritdoc cref="Course.Description"/>
    public string DescriptionError => Errors.ContainsKey(nameof(Description)) ? Errors[nameof(Description)].First() : "";

    private string notes;
    /// <inheritdoc cref="Course.Notes"/>
    public string Notes
    {
      get { return notes; }
      set
      {
        SetOrError(new()
        {
          new Tuple<bool, string>(value.IsEmpty() || value.Length <= 2500, "Notes Max 2500 Characters")
        }, value.TrimFix());

        SetProperty(ref notes, value.TrimFix());
      }
    }

    /// <inheritdoc cref="Course.Notes"/>
    public string NotesError => Errors.ContainsKey(nameof(Notes)) ? Errors[nameof(Notes)].First() : "";

    private DateTime start;
    /// <inheritdoc cref="Course.Start"/>
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

    /// <inheritdoc cref="Course.Start"/>
    public string StartError => Errors.ContainsKey(nameof(Start)) ? Errors[nameof(Start)].First() : "";

    private DateTime end;
    /// <inheritdoc cref="Course.End"/>
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

    /// <inheritdoc cref="Course.End"/>
    public string EndError => Errors.ContainsKey(nameof(End)) ? Errors[nameof(End)].First() : "";

    private Instructor instructor;
    /// <inheritdoc cref="Course.InstructorId"/>
    public Instructor Instructor
    {
      get { return instructor; }
      set
      {
        SetOrError(new() { new Tuple<bool, string>(value.Id > 0, "An Instructor is required") }, value.Id,
                                                                                          nameof(Course.InstructorId));

        SetProperty(ref instructor, value);
      }
    }

    /// <inheritdoc cref="Course.InstructorId"/>
    public string InstructorIdError => Errors.ContainsKey(nameof(Course.InstructorId)) ?
                                                                        Errors[nameof(Course.InstructorId)].First() : "";

    private AcademicTerm term;
    /// <inheritdoc cref="Course.AcademicTermId"/>
    public AcademicTerm Term
    {
      get { return term; }
      set
      {
        SetOrError(new() { new Tuple<bool, string>(value.Id > 0, "A Term is required") }, value.Id,
                                                                                      nameof(Course.AcademicTermId));

        SetProperty(ref term, value);
      }
    }

    /// <inheritdoc cref="Course.AcademicTermId"/>
    public string AcademicTermIdError => Errors.ContainsKey(nameof(Course.AcademicTermId)) ?
                                                                    Errors[nameof(Course.AcademicTermId)].First() : "";

    private int? perfAssessmentId;
    /// <inheritdoc cref="Course.PerfAssessmentId"/>
    public int? PerfAssessmentId
    {
      get { return perfAssessmentId; }
      set
      {
        SetOrError(new() { new Tuple<bool, string>(true, "Performance Assessment") }, value);

        SetProperty(ref perfAssessmentId, value);
      }
    }

    private int? objAssessmentId;
    /// <inheritdoc cref="Course.ObjAssessmentId"/>
    public int? ObjAssessmentId
    {
      get { return objAssessmentId; }
      set
      {
        SetOrError(new() { new Tuple<bool, string>(true, "Objective Assessment") }, value);

        SetProperty(ref objAssessmentId, value);
      }
    }

    private CourseStatus status;
    /// <inheritdoc cref="Course.Status"/>
    public CourseStatus Status
    {
      get { return status; }
      set
      {
        SetOrError(new() { new Tuple<bool, string>(true, "Course Status") }, value);

        SetProperty(ref status, value);
      }
    }

    private Assessment perfAssessment;
    /// <inheritdoc cref="Course.PerfAssessmentId"/>
    public Assessment PerfAssessment
    {
      get { return perfAssessment; }
      set
      {
        SetProperty(ref perfAssessment, value);
      }
    }

    private Assessment objAssessment;
    /// <inheritdoc cref="Course.ObjAssessmentId"/>
    public Assessment ObjAssessment
    {
      get { return objAssessment; }
      set
      {
        SetProperty(ref objAssessment, value);
      }
    }

    /// <summary>
    /// All Course Status Types as a List
    /// </summary>
    public ObservableCollection<CourseStatus> Statuses { get; set; } = new ObservableCollection<CourseStatus>(StringExtension.GetEnumList<CourseStatus>());

    /// <summary>
    /// List of Instructors at WGU
    /// </summary>
    public ObservableCollection<Instructor> Instructors { get; set; } = new ObservableCollection<Instructor>();

    /// <summary>
    /// List of Academic Terms at WGU
    /// </summary>
    public ObservableCollection<AcademicTerm> Terms { get; set; } = new ObservableCollection<AcademicTerm>();

    /// <inheritdoc cref="CourseCUDVM" />
    public CourseCUDVM()
    {
      if (Id == null)
      {
        Title = "New Course";
        Name = null;
        Description = null;
        Notes = null;
        Start = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                                                12, 0, 0, DateTimeKind.Utc);
        End = new(DateTime.Now.Year, DateTime.Now.Month + 1, DateTime.Now.Day,
                                                                12, 0, 0, DateTimeKind.Utc);
      }

      PACommand = new Command(async () => await OnPANavigate());
      OACommand = new Command(async () => await OnOANavigate());
    }

    /// <inheritdoc cref="CourseCUDVM" />
    public CourseCUDVM(Func<Task> save, Func<Task> delete, Func<Task> share) : base(save, delete)
    {
      Service = DependencyService.Get<ICourseService>();
      _termService = DependencyService.Get<IAcademicTermService>();
      _instructorService = DependencyService.Get<IInstructorService>();
      _assessmentService = DependencyService.Get<IAssessmentService>();

      LoadAsync().ConfigureAwait(true);

      if (Id == null)
      {
        Title = "New Course";
        Name = null;
        Description = null;
        Notes = null;
        Start = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                                                12, 0, 0, DateTimeKind.Utc);
        End = new(DateTime.Now.Year, DateTime.Now.Month + 1, DateTime.Now.Day,
                                                                12, 0, 0, DateTimeKind.Utc);
        Status = Statuses.Where(pr => pr == CourseStatus.PlanToTake).FirstOrDefault();
        SetOrError(new() { new Tuple<bool, string>(-1 > 0, "An Instructor is required") }, -1,
                                                                                          nameof(Course.InstructorId));
        SetOrError(new() { new Tuple<bool, string>(-1 > 0, "A Term is required") }, -1,
                                                                                      nameof(Course.AcademicTermId));
      }

      PACommand = new Command(async () => await OnPANavigate());
      OACommand = new Command(async () => await OnOANavigate());
      ShareCommand = new Command(async () => await share?.Invoke());

      IsBusy = false;
    }

    public async Task LoadCourse(int id)
    {
      IsBusy = true;
      Id = id;
      Course course = null;

      await Service.Get(pr => pr.Id == id).ContinueWith(t =>
      {
        if (t.Exception == null)
        {
          Id = t.Result.Id;
          End = t.Result.End;
          Title = $"Course {id}";
          Name = t.Result.Name;
          Description = t.Result.Description;
          Notes = t.Result.Notes;
          Start = t.Result.Start;
          End = t.Result.End;
          Status = Statuses.Where(pr => pr == t.Result.Status).FirstOrDefault();
          ObjAssessmentId = t.Result.ObjAssessmentId;
          PerfAssessmentId = t.Result.PerfAssessmentId;
          if (Instructors.Count > 0)
            Instructor = Instructors.FirstOrDefault(pr => pr.Id == t.Result.InstructorId);
          if (Terms.Count > 0)
            Term = Terms.FirstOrDefault(pr => pr.Id == t.Result.AcademicTermId);
          Item = t.Result;
          course = t.Result;
        }
      }).ConfigureAwait(true);

      if (course == null)
      {
        Title = "New Course";
        Name = null;
        Description = null;
        Notes = null;
        Start = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                                                12, 0, 0, DateTimeKind.Utc);
        End = new(DateTime.Now.Year, DateTime.Now.Month + 1, DateTime.Now.Day,
                                                                12, 0, 0, DateTimeKind.Utc);
        Status = Statuses.FirstOrDefault();
        SetOrError(new() { new Tuple<bool, string>(-1 > 0, "An Instructor is required") }, -1,
                                                                                          nameof(Course.InstructorId));
        SetOrError(new() { new Tuple<bool, string>(-1 > 0, "A Term is required") }, -1,
                                                                                      nameof(Course.AcademicTermId));
      }

      if (ObjAssessmentId.HasValue && PerfAssessmentId.HasValue)
      {
        Task<Assessment> oaTask = _assessmentService.Get(pr => pr.Id == ObjAssessmentId.Value);
        Task<Assessment> perfTask = _assessmentService.Get(pr => pr.Id == PerfAssessmentId.Value);

        await Task.WhenAll(oaTask, perfTask).ConfigureAwait(true);

        ObjAssessment = oaTask.Result;
        PerfAssessment = perfTask.Result;
      }
      else if (ObjAssessmentId.HasValue && !PerfAssessmentId.HasValue)
      {
        ObjAssessment = await _assessmentService.Get(pr => pr.Id == ObjAssessmentId.Value).ConfigureAwait(true);
      }
      else if (PerfAssessmentId.HasValue && !ObjAssessmentId.HasValue)
      {
        PerfAssessment = await _assessmentService.Get(pr => pr.Id == PerfAssessmentId.Value).ConfigureAwait(true);
      }
    }

    private async Task LoadAsync()
    {
      Instructors.Clear();
      Terms.Clear();

      List<Instructor> instructors = new();
      List<AcademicTerm> terms = new();

      Task<List<Instructor>> instructorTask = _instructorService.GetAll();
      Task<List<AcademicTerm>> termTask = _termService.GetAll();

      await Task.WhenAll(instructorTask, termTask).ConfigureAwait(true);

      if (instructorTask.Exception == null)
        instructors = instructorTask.Result;

      if (termTask.Exception == null)
        terms = termTask.Result;

      foreach (Instructor instructor in instructors.OrderBy(pr => pr.Name))
        Instructors.Add(instructor);

      foreach (AcademicTerm term in terms.OrderBy(pr => pr.TermTitle))
        Terms.Add(term);

      if (Id != null)
      {
        Instructor = Instructors.FirstOrDefault(t => t.Id == Item.InstructorId);
        Term = Terms.FirstOrDefault(t => t.Id == Item.AcademicTermId);
      }
      else
      {
        Status = Statuses.FirstOrDefault();
        SetOrError(new() { new Tuple<bool, string>(-1 > 0, "An Instructor is required") }, -1,
                                                                                            nameof(Course.InstructorId));
        SetOrError(new() { new Tuple<bool, string>(-1 > 0, "A Term is required") }, -1,
                                                                                        nameof(Course.AcademicTermId));
      }
    }

    private async Task OnPANavigate()
    {
      if (Item.Id > 0)
        await Shell.Current.GoToAsync($"{nameof(AssessmentCUDPage)}?CourseId={Item.Id}&Id={Item.PerfAssessmentId}&OAPA=Performance");
    }

    private async Task OnOANavigate()
    {
      if (Item.Id > 0)
        await Shell.Current.GoToAsync($"{nameof(AssessmentCUDPage)}?CourseId={Item.Id}&Id={Item.ObjAssessmentId}&OAPA=Objective");
    }
  }
}

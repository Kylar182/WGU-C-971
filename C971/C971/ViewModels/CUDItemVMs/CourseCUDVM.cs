using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using C971.Extensions;
using C971.Models.DatabaseModels;
using C971.Models.Enums;
using C971.Services;
using Xamarin.Forms;

namespace C971.ViewModels.ItemCUDVMs
{
  /// <summary>
  /// VM For a Academic Course Create / Update / Delete
  /// </summary>
  public class CourseCUDVM : BaseRUDPageVM<Course>
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
          new Tuple<bool, string>(!value.NotEmpty() || value.Length <= 150, "Name Max 150 Characters")
        }, value.TrimFix());

        SetProperty(ref name, value.TrimFix());
      }
    }

    /// <inheritdoc cref="Course.Name"/>
    public string NameError => Errors.ContainsKey(nameof(Name)) ? Errors[nameof(Name)].First() : "";

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
          new Tuple<bool, string>(!value.NotEmpty() || value.Length <= 1000, "Description Max 1000 Characters")
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
          new Tuple<bool, string>(!value.NotEmpty() || value.Length <= 2500, "Notes Max 2500 Characters")
        },
                                                                                                      value.TrimFix());

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
        SetOrError(new() { new Tuple<bool, string>(val < End, "Start must be earlier than End") }, val);

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
        val = val.AddTicks(-1);

        SetOrError(new() { new Tuple<bool, string>(val > Start, "End must be later than Start") }, val);

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

    private Tuple<CourseStatus, string> status;
    /// <inheritdoc cref="Course.Status"/>
    public Tuple<CourseStatus, string> Status
    {
      get { return status; }
      set
      {
        SetOrError(new() { new Tuple<bool, string>(true, "Course Status") }, value.Item1);

        SetProperty(ref status, value);
      }
    }

    /// <summary>
    /// List of Instructors at WGU
    /// </summary>
    public ObservableCollection<Tuple<CourseStatus, string>> Statuses { get; set; } = new ObservableCollection<Tuple<CourseStatus, string>>(StringExtension.GetEnumTuples<CourseStatus>());

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
    }

    /// <inheritdoc cref="CourseCUDVM" />
    public CourseCUDVM(Func<Task> save) : base(save)
    {
      Service = DependencyService.Get<ICourseService>();
      _termService = DependencyService.Get<IAcademicTermService>();
      _instructorService = DependencyService.Get<IInstructorService>();
      _assessmentService = DependencyService.Get<IAssessmentService>();

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
        Status = Statuses.FirstOrDefault();
        SetOrError(new() { new Tuple<bool, string>(-1 > 0, "An Instructor is required") }, -1,
                                                                                          nameof(Course.InstructorId));
        SetOrError(new() { new Tuple<bool, string>(-1 > 0, "A Term is required") }, -1,
                                                                                      nameof(Course.AcademicTermId));
      }

      LoadAsync().ConfigureAwait(true);

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
          course = t.Result;
        }
      }).ConfigureAwait(true);

      if (course != null)
      {
        Item = course;
        Title = $"Course {id}";
        Id = course.Id;
        Name = course.Name;
        Description = course.Description;
        Notes = course.Notes;
        Start = course.Start;
        End = course.End;
        Status = Statuses.Where(pr => pr.Item1 == course.Status).FirstOrDefault();
        ObjAssessmentId = course.ObjAssessmentId;
        PerfAssessmentId = course.PerfAssessmentId;
        if (Instructors.Count > 0)
          Instructor = Instructors.FirstOrDefault(t => t.Id == course.InstructorId);
        if (Terms.Count > 0)
          Term = Terms.FirstOrDefault(t => t.Id == course.AcademicTermId);
      }
      else
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
    }

    private async Task LoadAsync()
    {
      Instructors.Clear();
      Terms.Clear();

      List<Instructor> instructors = new();
      List<AcademicTerm> terms = new();

      await _instructorService.GetAll().ContinueWith(t =>
      {
        if (t.Exception == null)
        {
          instructors = t.Result;
        }
      }).ConfigureAwait(true);

      await _termService.GetAll().ContinueWith(t =>
      {
        if (t.Exception == null)
        {
          terms = t.Result;
        }
      }).ConfigureAwait(true);

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
        SetOrError(new() { new Tuple<bool, string>(-1 > 0, "An Instructor is required") }, -1,
                                                                                            nameof(Course.InstructorId));
        SetOrError(new() { new Tuple<bool, string>(-1 > 0, "A Term is required") }, -1,
                                                                                        nameof(Course.AcademicTermId));
      }
    }
  }
}

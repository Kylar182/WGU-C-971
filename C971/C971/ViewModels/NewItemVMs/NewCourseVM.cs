using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using C971.Extensions;
using C971.Models.DatabaseModels;
using C971.Services;
using Xamarin.Forms;

namespace C971.ViewModels.NewItemVMs
{
  /// <summary>
  /// VM For a New Academic Course
  /// </summary>
  public class NewCourseVM : BaseAddPageVM<Course>
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
        SetOrError(new() { new Tuple<bool, string>(value.NotEmpty(), "A Name is required") }, value.TrimFix());

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
        bool max = true;
        if (value.NotEmpty())
          max = value.Length <= 1000;

        SetOrError(new()
        {
          new Tuple<bool, string>(value.NotEmpty(), "A Description is required"),
          new Tuple<bool, string>(max, "Description Max 1000 Characters")
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
        bool max = true;
        if (value.NotEmpty())
          max = value.Length <= 2500;

        SetOrError(new() { new Tuple<bool, string>(max, "Notes Max 2500 Characters") }, value.TrimFix());

        SetProperty(ref notes, value.TrimFix());
      }
    }

    /// <inheritdoc cref="Course.Notes"/>
    public string NotesError => Errors.ContainsKey(nameof(Notes)) ? Errors[nameof(Notes)].First() : "";

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

    private int? id;
    /// <inheritdoc cref="BaseModel.Id"/>
    public int? Id
    {
      get { return id; }
      set
      {
        SetOrError(new() { new Tuple<bool, string>(true, "Couse Id") }, value);

        SetProperty(ref id, value);
      }
    }

    /// <summary>
    /// List of Instructors at WGU
    /// </summary>
    public ObservableCollection<Instructor> Instructors { get; set; } = new ObservableCollection<Instructor>();

    /// <summary>
    /// List of Academic Terms at WGU
    /// </summary>
    public ObservableCollection<AcademicTerm> Terms { get; set; } = new ObservableCollection<AcademicTerm>();

    /// <inheritdoc cref="NewCourseVM" />
    public NewCourseVM()
    {
      Title = "New Course";
      Name = null;

      if (Id == null)
      {
        Name = null;
        Description = null;
        Notes = null;
      }
    }

    /// <inheritdoc cref="NewCourseVM" />
    public NewCourseVM(Func<Task> save, int? id = null) : base(save)
    {
      Title = "New Course";
      Service = DependencyService.Get<ICourseService>();
      _termService = DependencyService.Get<IAcademicTermService>();
      _instructorService = DependencyService.Get<IInstructorService>();
      _assessmentService = DependencyService.Get<IAssessmentService>();

      if (id == null)
      {
        Name = null;
        Description = null;
        Notes = null;
        SetOrError(new() { new Tuple<bool, string>(-1 > 0, "An Instructor is required") }, -1,
                                                                                          nameof(Course.InstructorId));
        SetOrError(new() { new Tuple<bool, string>(-1 > 0, "A Term is required") }, -1,
                                                                                      nameof(Course.AcademicTermId));
      }

      LoadAsync().ConfigureAwait(true);

      IsBusy = false;
    }

    public async Task LoadCourseAsync(int? id)
    {
      Course course = await Service.Get(pr => pr.Id == id.Value);

      if (course != null)
      {
        Item = course;
        Id = id;
        Name = course.Name;
        Description = course.Description;
        Notes = course.Notes;
        ObjAssessmentId = course.ObjAssessmentId;
        PerfAssessmentId = course.PerfAssessmentId;
      }
      else
      {
        Title = "New Course";
        Name = null;
        Description = null;
        Notes = null;
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

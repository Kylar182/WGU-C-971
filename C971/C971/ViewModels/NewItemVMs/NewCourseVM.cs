using System;
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

    private int instructorId;
    /// <inheritdoc cref="Course.InstructorId"/>
    public int InstructorId
    {
      get { return instructorId; }
      set
      {
        SetOrError(new() { new Tuple<bool, string>(instructorId > 0, "An Instructor is required") }, value);

        SetProperty(ref instructorId, value);
      }
    }

    /// <inheritdoc cref="Course.InstructorId"/>
    public string InstructorIdError => Errors.ContainsKey(nameof(InstructorId)) ? Errors[nameof(InstructorId)].First() : "";

    /// <inheritdoc cref="NewCourseVM" />
    public NewCourseVM()
    {
      Title = "New Course";
      Name = null;
      Service = DependencyService.Get<ICourseService>();
      Description = null;
      Notes = null;
      InstructorId = 0;
    }

    /// <inheritdoc cref="NewCourseVM" />
    public NewCourseVM(Func<Task> save) : base(save)
    {
      Title = "New Course";
      Name = null;
      Service = DependencyService.Get<ICourseService>();
      Description = null;
      Notes = null;
      InstructorId = 0;
    }
  }
}

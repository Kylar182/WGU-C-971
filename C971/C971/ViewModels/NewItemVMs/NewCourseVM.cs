﻿using System;
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

    private int academicTermId;
    /// <inheritdoc cref="Course.AcademicTermId"/>
    public int AcademicTermId
    {
      get { return academicTermId; }
      set
      {
        SetOrError(new() { new Tuple<bool, string>(academicTermId > 0, "A Term is required") }, value);

        SetProperty(ref academicTermId, value);
      }
    }

    /// <inheritdoc cref="Course.AcademicTermId"/>
    public string AcademicTermIdError => Errors.ContainsKey(nameof(AcademicTermId)) ? Errors[nameof(AcademicTermId)].First() : "";

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
      IsBusy = true;
      Title = "New Course";
      Name = null;

      if (id == null)
      {
        Name = null;
        Description = null;
        Notes = null;
        InstructorId = -1;
        AcademicTermId = -1;
      }

      IsBusy = false;
    }

    /// <inheritdoc cref="NewCourseVM" />
    public NewCourseVM(Func<Task> save, int? id = null) : base(save)
    {
      IsBusy = true;
      Title = "New Course";
      Service = DependencyService.Get<ICourseService>();
      _termService = DependencyService.Get<IAcademicTermService>();
      _instructorService = DependencyService.Get<IInstructorService>();
      _assessmentService = DependencyService.Get<IAssessmentService>();

      LoadAsync().ConfigureAwait(true);

      if (id == null)
      {
        Name = null;
        Description = null;
        Notes = null;
        InstructorId = -1;
        AcademicTermId = -1;
      }

      IsBusy = false;
    }

    public async Task LoadCourseAsync(int? id)
    {
      Course course = await Service.Get(pr => pr.Id == id.Value);

      if (course != null)
      {
        Id = id;
        Name = course.Name;
        Description = course.Description;
        Notes = course.Notes;
        InstructorId = course.InstructorId;
        AcademicTermId = course.AcademicTermId ?? 0;
        ObjAssessmentId = course.ObjAssessmentId;
        PerfAssessmentId = course.PerfAssessmentId;
      }
      else
      {
        Title = "New Course";
        Name = null;
        Description = null;
        Notes = null;
        InstructorId = -1;
        AcademicTermId = -1;
      }
    }

    private async Task LoadAsync()
    {
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

      Instructors.Clear();

      foreach (Instructor instructor in instructors)
        Instructors.Add(instructor);

      Terms.Clear();

      foreach (AcademicTerm term in terms)
        Terms.Add(term);
    }
  }
}

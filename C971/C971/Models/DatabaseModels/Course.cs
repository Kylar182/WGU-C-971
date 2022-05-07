using System;
using C971.Models.Enums;
using SQLite;

namespace C971.Models.DatabaseModels
{
  /// <summary>
  /// A WGU Academic Course
  /// </summary>
  [Table(nameof(Course) + "s")]
  public class Course : BaseModel, INotify
  {
    /// <summary>
    /// Name of the Course
    /// </summary>
    [MaxLength(150), Unique, NotNull]
    public string Name { get; set; }
    /// <summary>
    /// Description of the Course
    /// </summary>
    [MaxLength(1500), NotNull]
    public string Description { get; set; }
    /// <summary>
    /// User's written Notes on the Course
    /// </summary>
    [MaxLength(2500)]
    public string Notes { get; set; }

    /// <summary>
    /// Foreign Key matching this Course to an Academic Term (if any)
    /// </summary>
    public int? AcademicTermId { get; set; }
    /// <summary>
    /// Foreign Key matching this Course's Performance Assessment (if any)
    /// </summary>
    public int? PerfAssessmentId { get; set; }
    /// <summary>
    /// Foreign Key matching this Course's Objective Assessment (if any)
    /// </summary>
    public int? ObjAssessmentId { get; set; }

    /// <summary>
    /// Foreign Key matching this Course to an Academic Instructor
    /// </summary>
    public int InstructorId { get; set; }

    /// <summary>
    /// Date / Time of the Academic Course's Start, Universal Time
    /// </summary>
    [NotNull]
    public DateTime Start { get; set; }
    /// <summary>
    /// Date / Time of the Academic Course's End, Universal Time
    /// </summary>
    [NotNull]
    public DateTime End { get; set; }

    /// <summary>
    /// WGU Academic Course Status for this Course
    /// </summary>
    public CourseStatus Status { get; set; }

    /// <inheritdoc />
    public int StartId { get; set; }
    /// <inheritdoc />
    public int EndId { get; set; }

    /// <inheritdoc />
    public Notification StartNotification()
    {
      return new()
      {
        Id = StartId,
        Title = Name,
        Display = Start
      };
    }

    /// <inheritdoc />
    public Notification EndNotification()
    {
      return new()
      {
        Id = EndId,
        Title = Name,
        Display = End
      };
    }
  }
}

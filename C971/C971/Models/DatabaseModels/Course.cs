using System;
using SQLite;

namespace C971.Models.DatabaseModels
{
  /// <summary>
  /// A WGU Academic Course Period
  /// </summary>
  [Table(nameof(Course) + "s")]
  public class Course : BaseModel
  {
    /// <summary>
    /// Name of the Course
    /// </summary>
    [MaxLength(150), Unique, NotNull]
    public string Name { get; set; }

    /// <summary>
    /// Foreign Key matching this Course to an Academic Term (if any)
    /// </summary>
    public int? AcademicTermId { get; set; }

    /// <summary>
    /// Foreign Key matching this Course to an Academic Instructor
    /// </summary>
    public int InstructorId { get; set; }

    /// <summary>
    /// Date / Time of the Academic Term's Start (WGU / Utah) Local Time
    /// </summary>
    [MaxLength(150), NotNull]
    public DateTime Start { get; set; }
    /// <summary>
    /// Date / Time of the Academic Term's End (WGU / Utah) Local Time
    /// </summary>
    [MaxLength(150), NotNull]
    public DateTime End { get; set; }
  }
}

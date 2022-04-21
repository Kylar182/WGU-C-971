using System;
using SQLite;

namespace C971.Models.DatabaseModels
{
  /// <summary>
  /// A WGU Academic Term / Course Period
  /// </summary>
  /// <remarks>
  /// Typically Six Month Increments
  /// </remarks>
  [Table(nameof(AcademicTerm) + "s")]
  public class AcademicTerm : BaseModel
  {
    /// <summary>
    /// The Term Title, must be Unique
    /// </summary>
    [MaxLength(250), Unique, NotNull]
    public string TermTitle { get; set; } = "";

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

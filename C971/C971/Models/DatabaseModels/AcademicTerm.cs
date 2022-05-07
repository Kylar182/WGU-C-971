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
    /// Date / Time of the Academic Term's Start, Universal Time
    /// </summary>
    [MaxLength(150), NotNull]
    public DateTime Start { get; set; }
    /// <summary>
    /// Date / Time of the Academic Term's End, Universal Time
    /// </summary>
    [MaxLength(150), NotNull]
    public DateTime End { get; set; }
  }
}

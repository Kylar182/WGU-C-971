using System;
using SQLite;

namespace C971.Models.DatabaseModels
{
  /// <summary>
  /// A WGU Academic Course
  /// </summary>
  [Table(nameof(Assessment) + "s")]
  public class Assessment : BaseModel, INotify
  {
    /// <summary>
    /// Name of the Assessment
    /// </summary>
    [MaxLength(150), NotNull]
    public string Name { get; set; }

    /// <summary>
    /// Foreign Key matching this Assessment to an Academic Course
    /// </summary>
    public int CourseId { get; set; }

    /// <summary>
    /// Date / Time the Assessment is started, Universal Time
    /// </summary>
    [NotNull]
    public DateTime Start { get; set; }

    /// <summary>
    /// Date / Time the Assessment is due, Universal Time
    /// </summary>
    [NotNull]
    public DateTime End { get; set; }

    /// <inheritdoc />
    public int NotificationId { get; set; }

    /// <inheritdoc />
    public Notification Notification()
    {
      return new()
      {
        Id = NotificationId,
        Title = Name,
        Start = Start
      };
    }
  }
}

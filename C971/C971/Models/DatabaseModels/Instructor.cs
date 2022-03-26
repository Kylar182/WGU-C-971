using SQLite;

namespace C971.Models.DatabaseModels
{
  /// <summary>
  /// A Person that instructs Courses at WGU
  /// </summary>
  [Table(nameof(Instructor) + "s")]
  public class Instructor : BaseModel
  {
    /// <summary>
    /// Given name of the Instructor
    /// </summary>
    [MaxLength(150), Unique, NotNull]
    public string Name { get; set; }
    /// <summary>
    /// The Instructor's Phone Number
    /// </summary>
    [MaxLength(150), Unique, NotNull]
    public string PhoneNumber { get; set; }
    /// <summary>
    /// The Instructor's Email
    /// </summary>
    [MaxLength(150), Unique, NotNull]
    public string Email { get; set; }
  }
}

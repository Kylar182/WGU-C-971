using SQLite;

namespace C971.Models.DatabaseModels
{
  /// <summary>
  /// Base Model for all Database Models
  /// </summary>
  public class BaseModel
  {
    /// <summary>
    /// Model's Unique Identifier / Table Key
    /// </summary>
    [PrimaryKey, AutoIncrement, Unique, NotNull, Column(nameof(BaseModel.Id))]
    public int Id { get; set; }
  }
}

using System;
using Plugin.LocalNotifications;
using SQLite;

namespace C971.Models.DatabaseModels
{
  /// <summary>
  /// A WGU Academic Course
  /// </summary>
  [Table(nameof(Notification) + "s")]
  public class Notification : BaseModel
  {
    /// <summary>
    /// Title of the Notification
    /// </summary>
    [MaxLength(150), NotNull]
    public string Title { get; set; }

    /// <summary>
    /// Date the Notification should Display, Universal Time
    /// </summary>
    public DateTime Start { get; set; }

    public void Insert()
    {
      CrossLocalNotifications.Current.Show(Title, $"{Title} starts today!", Id, Start);
    }
  }
}

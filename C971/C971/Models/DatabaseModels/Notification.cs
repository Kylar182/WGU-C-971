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

    /// <summary>
    /// Add this Notification to the Local Notifications
    /// </summary>
    public void Insert()
    {
      CrossLocalNotifications.Current.Show(Title, $"{Title} starts today!", Id, Start);
    }

    /// <summary>
    /// Update this Notification in the Local Notifications
    /// </summary>
    public void Update()
    {
      Cancel();
      CrossLocalNotifications.Current.Show(Title, $"{Title} starts today!", Id, Start);
    }

    /// <summary>
    /// Remove this Notification from the Local Notifications
    /// </summary>
    public void Cancel()
    {
      CrossLocalNotifications.Current.Cancel(Id);
    }
  }
}

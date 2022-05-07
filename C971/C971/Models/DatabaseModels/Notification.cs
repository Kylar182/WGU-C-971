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
    public DateTime Display { get; set; }

    /// <summary>
    /// Add this Start Notification to the Local Notifications
    /// </summary>
    public void StartInsert()
    {
      CrossLocalNotifications.Current.Show(Title, $"{Title} starts today!", Id, Display);
    }

    /// <summary>
    /// Add this End Notification to the Local Notifications
    /// </summary>
    public void EndInsert()
    {
      CrossLocalNotifications.Current.Show(Title, $"{Title} ends today!", Id, Display);
    }

    /// <summary>
    /// Update this Start Notification in the Local Notifications
    /// </summary>
    public void StartUpdate()
    {
      Cancel();
      CrossLocalNotifications.Current.Show(Title, $"{Title} starts today!", Id, Display);
    }

    /// <summary>
    /// Update this End Notification in the Local Notifications
    /// </summary>
    public void EndUpdate()
    {
      Cancel();
      CrossLocalNotifications.Current.Show(Title, $"{Title} ends today!", Id, Display);
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

using System;

namespace C971.Models.DatabaseModels
{
  /// <summary>
  /// Interface for Models that require a Notification to implement
  /// </summary>
  public interface INotify
  {
    /// <summary>
    /// Foreign Key to the Notification
    /// </summary>
    public int NotificationId { get; set; }

    /// <summary>
    /// Date the Notification should Display, Universal Time
    /// </summary>
    public DateTime Start { get; set; }

    /// <summary>
    /// Creates and Returns a Notification for this Implementation
    /// </summary>
    public Notification Notification();
  }
}

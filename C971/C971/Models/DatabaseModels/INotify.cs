using System;

namespace C971.Models.DatabaseModels
{
  /// <summary>
  /// Interface for Models that require a Notification to implement
  /// </summary>
  public interface INotify
  {
    /// <summary>
    /// Foreign Key to the Start Notification
    /// </summary>
    public int StartId { get; set; }
    /// <summary>
    /// Foreign Key to the End Notification
    /// </summary>
    public int EndId { get; set; }

    /// <summary>
    /// Date the Start Notification should Display, Universal Time
    /// </summary>
    public DateTime Start { get; set; }
    /// <summary>
    /// Date the End Notification should Display, Universal Time
    /// </summary>
    public DateTime End { get; set; }

    /// <summary>
    /// Creates and Returns a Start Notification for this Implementation
    /// </summary>
    public Notification StartNotification();

    /// <summary>
    /// Creates and Returns a End Notification for this Implementation
    /// </summary>
    public Notification EndNotification();
  }
}

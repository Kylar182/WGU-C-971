using System;

namespace C971.Models.Enums
{
  /// <summary>
  /// A WGU Academic Course Status
  /// </summary>
  [Flags]
  public enum CourseStatus
  {
    /// <summary>
    /// User has not chosen a status for this Course
    /// </summary>
    None = 0,
    /// <summary>
    /// Course is currently in Progress for this User
    /// </summary>
    InProgress = 1,
    /// <summary>
    /// Course is currently in Completed for this User
    /// </summary>
    Completed = 2,
    /// <summary>
    /// Course has been Dropped by this User
    /// </summary>
    Dropped = 3,
    /// <summary>
    /// User has not taken but Plans to take this Course
    /// </summary>
    PlanToTake = 4
  }
}

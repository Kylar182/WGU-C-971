using System;
using System.Reflection;

namespace C971.Extensions
{
  /// <summary>
  /// Static Class with Methods for Property Name Extensions
  /// </summary>
  public static class ByName
  {
    /// <summary>
    /// Method to Try and set a Value on a property by the property's name
    /// </summary>
    /// <param name="obj">Object that need's set</param>
    /// <param name="name">Property's Name</param>
    /// <param name="value">Output of object</param>
    public static bool SetByName(this Object obj, string name, Object value)
    {
      PropertyInfo prop = obj.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
      if (null == prop || !prop.CanWrite) return false;
      prop.SetValue(obj, value, null);
      return true;
    }


    public static object GetByName(this Object obj, string name)
    {
      PropertyInfo prop = obj.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
      if (null == prop || !prop.CanWrite) return null;

      if (prop.GetValue(obj, null) == null) return null;

      return prop.GetValue(obj, null);
    }
  }
}

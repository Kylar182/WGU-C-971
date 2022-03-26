using System;
using System.ComponentModel;

namespace C971.Extensions
{
  /// <summary>
  /// T Type Extension Methods
  /// </summary>
  public static class TExtensions
  {
    /// <summary>
    /// Parse Value as String to T Value
    /// </summary>
    public static bool GenericTryParse<T>(this string input, out T value)
    {
      var converter = TypeDescriptor.GetConverter(typeof(T));

      if (converter != null && converter.IsValid(input))
      {
        value = (T)converter.ConvertFromString(input);
        return true;
      }

      if (typeof(T).IsValueType && Nullable.GetUnderlyingType(typeof(T)) == null)
        value = (T)typeof(T).GetDefaultValue();
      else
        value = default(T);

      return false;
    }

    /// <summary>
    /// Gets the default value of the Given Type
    /// </summary>
    /// <param name="t">
    /// Type to Extend and return the Default value of
    /// </param>
    public static object GetDefaultValue(this Type t)
    {
      if (t.IsValueType && Nullable.GetUnderlyingType(t) == null)
        return Activator.CreateInstance(t);

      return null;
    }
  }
}

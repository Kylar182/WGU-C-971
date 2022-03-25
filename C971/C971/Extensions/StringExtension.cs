namespace C971.Extensions
{
  /// <summary>
  /// String Extension Class Methods
  /// </summary>
  public static class StringExtension
  {
    /// <summary>
    /// Trims the String and returns Null if it's empty space
    /// </summary>
    public static string TrimFix(this string rawString)
    {
      if (!string.IsNullOrWhiteSpace(rawString))
        return rawString.Trim();

      return null;
    }

    /// <summary>
    /// Shortented IsNullOrWhiteSpace as Extension
    /// </summary>
    public static bool IsEmpty(this string str) => str.TrimFix() == null;

    /// <summary>
    /// Shortented / Reversed IsNullOrWhiteSpace as Extension
    /// </summary>
    public static bool NotEmpty(this string str) => str.TrimFix() != null;

    /// <summary>
    /// Pluralizes the Current string
    /// </summary>
    public static string ToPlural(this string str)
    {
      if (str.IsEmpty())
        return str;
      else if (str.EndsWith("es"))
        return str;
      else if (!str.EndsWith("s"))
        return str + "s";
      else
        return str + "es";
    }

    /// <summary>
    /// If string is not null/empty and longer than specified length, 
    /// it will be cut and returned as the specified length
    /// If it's not as long as the specified length it's returned as is
    /// If it's null / empty, '...' is returned
    /// </summary>
    public static string FriendlyLength(this string longString, int length)
    {
      if (longString.NotEmpty())
      {
        if (longString.Length > length)
          return longString.Substring(0, length);
        else
          return longString;
      }
      else
        return "...";
    }
  }
}

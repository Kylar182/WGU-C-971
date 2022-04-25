using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
    /// Splits at Caps to turn Pascal into a single word
    /// </summary>
    public static string SplitPascal(this string str)
    {
      if (str.IsEmpty())
        return str;
      else if (str.TrimFix().Length == 1)
        return str.TrimFix().ToUpper();
      else
        return Regex.Replace(str.TrimFix(), "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ").TrimEnd();
    }

    /// <summary>
    /// Creates a List of the given enum
    /// </summary>
    /// <typeparam name="TValue">
    /// Enum type to convert
    /// </typeparam>
    public static List<TValue> GetEnumList<TValue>() where TValue : Enum
    {
      return Enum.GetValues(typeof(TValue)).Cast<TValue>().ToList();
    }

    /// <summary>
    /// Creates Tuples from the given Enum type using the Enum values and their Display values
    /// </summary>
    /// <typeparam name="TValue">
    /// Enum types to get a List of Tuples for
    /// </typeparam>
    public static List<Tuple<TValue, string>> GetEnumTuples<TValue>() where TValue : Enum
    {
      List<Tuple<TValue, string>> tupleNums = new();
      foreach (TValue tupleNum in GetEnumList<TValue>())
        tupleNums.Add(new Tuple<TValue, string>(tupleNum, tupleNum.ToString("g").SplitPascal()));

      return tupleNums;
    }

    /// <summary>
    /// Creates Tuples from the given Enum type using the Enum values and their Display values
    /// </summary>
    /// <typeparam name="TValue">
    /// Enum types to get a List of Tuples for
    /// </typeparam>
    public static Dictionary<TValue, string> GetEnumDictionary<TValue>() where TValue : Enum
    {
      Dictionary<TValue, string> dictionaryNums = new();
      foreach (TValue tupleNum in GetEnumList<TValue>())
        dictionaryNums.Add(tupleNum, tupleNum.ToString("g").SplitPascal());

      return dictionaryNums;
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

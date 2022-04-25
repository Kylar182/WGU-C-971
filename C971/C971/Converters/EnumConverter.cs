using System;
using System.Globalization;
using C971.Extensions;
using Xamarin.Forms;

namespace C971.Converters
{
  public class EnumConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is not Enum) return value;

      Enum enumValue = (Enum)value;
      return enumValue.ToString("g").SplitPascal();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}

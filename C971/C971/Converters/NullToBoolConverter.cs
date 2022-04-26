using System;
using System.Globalization;
using Xamarin.Forms;

namespace C971.Converters
{
  public class NullToBoolConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
        return false;
      else
        return true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
        return false;
      else
        return true;
    }
  }

  public class NotNullToBoolConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
        return true;
      else
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
        return true;
      else
        return false;
    }
  }
}

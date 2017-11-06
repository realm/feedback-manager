using System;
using System.Globalization;
using Realms.Sync;
using Xamarin.Forms;

namespace Converters
{
    public class IsResolvedToAlphaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue &&
                boolValue)
            {
                return 0.4;
            }

            return 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

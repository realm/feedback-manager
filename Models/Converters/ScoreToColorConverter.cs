using System;
using System.Globalization;
using Realms.Sync;
using Xamarin.Forms;

namespace Converters
{
    public class ScoreToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is float floatValue)
            {
                if (floatValue < 0.25f)
                {
                    return Color.FromHex("a80624");
                }
                else if (floatValue < 0.75f)
                {
                    return Color.FromHex("a2aa08");
                }
                else
                {
                    return Color.FromHex("09ba50");
                }
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

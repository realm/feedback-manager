using System;
using System.Globalization;
using Realms.Sync;
using Xamarin.Forms;

namespace Converters
{
    public class AuthorToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string authorId)
            {
                return authorId == User.Current.Identity ? Color.FromHex("FCC397") : Color.FromHex("EBEBF2");
            }

            return new Thickness();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

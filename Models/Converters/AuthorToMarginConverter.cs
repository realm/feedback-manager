using System;
using System.Globalization;
using Realms.Sync;
using Xamarin.Forms;

namespace Converters
{
    public class AuthorToMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string authorId)
            {
                return authorId == User.Current.Identity ? new Thickness(80, 3, 10, 3) : new Thickness(10, 3, 80, 3);
            }

            return new Thickness();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

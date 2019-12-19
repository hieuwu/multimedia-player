using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MultimediaPlayer
{
    public class NameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var info = value as string;
            var tokens = info.Split(new string[] { "." },
                StringSplitOptions.None);
            // Noi-buon-cua-anh.mp3
            // ["Noi-buon-cua-anh", "mp3"]

            return tokens[0];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

     
    }
}

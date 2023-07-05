using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public class NullSwitchStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values is null || values.Length == 0)
            {
                return string.Empty;
            }


            return values.Where(x => x is not null)
                         .Select(x => x.ToString()?[30..])
                         .First(x => !string.IsNullOrEmpty(x));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
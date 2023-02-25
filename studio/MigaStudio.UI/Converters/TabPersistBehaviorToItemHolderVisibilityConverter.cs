using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Acorisoft.FutureGL.MigaStudio.Controls.Basic;

namespace Acorisoft.FutureGL.MigaStudio.Converters
{
    public class TabPersistBehaviorToItemHolderVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var behavior = (TabPersistBehavior)value;
            switch (behavior)
            {
                default:
                    return Visibility.Collapsed;
                case TabPersistBehavior.All:
                case TabPersistBehavior.Timed:
                    return Visibility.Visible;

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

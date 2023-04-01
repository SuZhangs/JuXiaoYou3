using System;
using System.Globalization;
using System.Windows.Data;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
using Acorisoft.FutureGL.MigaStudio.Models.Modules;
using Acorisoft.FutureGL.MigaStudio.Models.Modules.ViewModels;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public class BlockNameConverter : IValueConverter, IMultiValueConverter
    {
        private const string Pattern = "{0}: {1}";

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var t = values[0] as ModuleBlockEditUI;
            var n = values[1]?.ToString();

            return string.Format(Pattern, SubSystemString.GetModuleBlockNameByType(t), n);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BlockType bt)
                return SubSystemString.GetModuleBlockNameByKind(bt);
            return SubSystemString.GetModuleBlockNameByType(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
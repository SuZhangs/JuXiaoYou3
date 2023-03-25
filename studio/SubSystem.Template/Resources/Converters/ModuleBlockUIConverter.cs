using System;
using System.Globalization;
using System.Windows.Data;
using Acorisoft.FutureGL.MigaDB.Data.Templates.Modules;
using Acorisoft.FutureGL.MigaStudio.Modules;
using Acorisoft.FutureGL.MigaStudio.Modules.ViewModels;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public class ModuleBlockUIConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ModuleBlock mb)
            {
                return ModuleBlockFactory.GetDataUI(mb);
            }
            
            if (value is ModuleBlockEditUI mbe)
            {
                return ModuleBlockFactory.GetDataUI(mbe.CreateInstance());
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
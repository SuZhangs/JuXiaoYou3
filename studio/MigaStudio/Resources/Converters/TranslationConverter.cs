using System;
using System.Globalization;
using System.Windows.Data;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Enums;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public class TranslationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                MainTheme mt => GetMainTheme(mt),
                CultureArea ca => GetCultureArea(ca),
                _ => string.Empty
            };
        }

        private static string GetMainTheme(MainTheme theme)
        {
            return theme switch
            {
                MainTheme.Dark => Language.Culture switch
                {
                    CultureArea.Chinese => "亮色模式",
                    _ => "Light Theme"
                },
                MainTheme.Custom => Language.Culture switch
                {
                    CultureArea.Chinese => "自定义主题",
                    _ => "Custom Theme"
                },
                _ => Language.Culture switch
                {
                    CultureArea.Chinese => "夜间模式",
                    _ => "Dark Theme"
                },
            };
        }
        
        private static string GetCultureArea(CultureArea theme)
        {
            return theme switch
            {
                CultureArea.English => "English",
                CultureArea.French => "Français",
                CultureArea.Japanese => "日本語",
                CultureArea.Korean => "한국어",
                CultureArea.Russian => "Русский",
                _ => "确定"
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
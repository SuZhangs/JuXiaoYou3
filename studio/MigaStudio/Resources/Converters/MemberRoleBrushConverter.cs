using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Acorisoft.FutureGL.MigaDB.Data.Socials;
using Acorisoft.FutureGL.MigaDB.Enums;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Converters
{
    public class MemberRoleBrushConverter : IValueConverter
    {
        public static readonly SolidColorBrush QQ_Owner   = new SolidColorBrush(Color.FromRgb(0xfe, 0xc8, 0x3d));
        public static readonly SolidColorBrush QQ_Manager = new SolidColorBrush(Color.FromRgb(0xfe, 0xc8, 0x3d));
        public static readonly SolidColorBrush QQ_Self    = new SolidColorBrush(Color.FromRgb(0xfe, 0xc8, 0x3d));
        public static readonly SolidColorBrush Default    = new SolidColorBrush(Colors.Transparent);
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MemberRole role)
            {
                return role switch
                {
                    MemberRole.Manager => QQ_Manager,
                    MemberRole.Special => QQ_Manager,
                    MemberRole.Owner   => QQ_Owner,
                    _                  => Default
                };
            }
            return Default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
    public class MemberRoleNameConverter : IValueConverter
    {
        public static readonly SolidColorBrush QQ_Owner   = new SolidColorBrush(Color.FromRgb(0xfe, 0xc8, 0x3d));
        public static readonly SolidColorBrush QQ_Manager = new SolidColorBrush(Color.FromRgb(0xfe, 0xc8, 0x3d));
        public static readonly SolidColorBrush QQ_Self    = new SolidColorBrush(Color.FromRgb(0xfe, 0xc8, 0x3d));
        public static readonly SolidColorBrush Default    = new SolidColorBrush(Colors.Transparent);
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MemberRole role)
            {
                return role switch
                {
                    MemberRole.Manager => QQ_Manager,
                    MemberRole.Special => QQ_Manager,
                    MemberRole.Owner   => QQ_Owner,
                    _                  => Default
                };
            }
            return Default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
    public class MemberRoleVisibilityConverter : IValueConverter
    {
        public static readonly object Collapsed = Visibility.Collapsed;
        public static readonly object Visible   = Visibility.Visible;
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MemberRole role)
            {
                return role switch
                {
                    MemberRole.Manager => Visible,
                    MemberRole.Special => Visible,
                    MemberRole.Owner   => Collapsed,
                    _                  => Collapsed
                };
            }
            return Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
using System.Windows.Markup;

using Acorisoft.FutureGL.Forest.Styles;

namespace Acorisoft.FutureGL.Forest.Markup
{
    public class ForegroundExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ThemeSystem.Instance.Theme.Colors[(int)ForestTheme.Foreground];
        }
    }

    public class ForegroundBrushExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ThemeSystem.Instance.Theme.Colors[(int)ForestTheme.Foreground].ToSolidColorBrush();
        }
    }


    public class BackgroundExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ThemeSystem.Instance.Theme.Colors[(int)ForestTheme.Background];
        }
    }

    public class BackgroundBrushExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ThemeSystem.Instance.Theme.Colors[(int)ForestTheme.Background].ToSolidColorBrush();
        }
    }
    
    public class DisabledExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ThemeSystem.Instance.Theme.Colors[(int)ForestTheme.BackgroundInactive];
        }
    }

    public class DisabledBrushExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ThemeSystem.Instance.Theme.Colors[(int)ForestTheme.BackgroundInactive].ToSolidColorBrush();
        }
    }
}
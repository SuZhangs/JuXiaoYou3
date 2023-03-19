using System.Windows.Markup;

namespace Acorisoft.FutureGL.Forest.Markup
{
    public class MaskBrushExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ThemeSystem.Instance.Theme.Colors[(int)ForestTheme.Mask100].ToSolidColorBrush();
        }
    }
    
    public class MaskExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ThemeSystem.Instance.Theme.Colors[(int)ForestTheme.Mask100];
        }
    }
    
    public class MaskDarkenExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ThemeSystem.Instance.Theme.Colors[(int)ForestTheme.Mask200];
        }
    }
    
    public class MaskDarkenBrushExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ThemeSystem.Instance.Theme.Colors[(int)ForestTheme.Mask200].ToSolidColorBrush();
        }
    }
}
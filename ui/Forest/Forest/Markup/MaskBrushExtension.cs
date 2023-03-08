using System.Windows.Markup;

using Acorisoft.FutureGL.Forest.Styles;

namespace Acorisoft.FutureGL.Forest.Markup
{
    public class MaskBrushExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ThemeSystem.Instance.Theme.Colors[(int)ForestTheme.Mask].ToSolidColorBrush();
        }
    }
    
    public class MaskDarkenBrushExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ThemeSystem.Instance.Theme.Colors[(int)ForestTheme.MaskDarken].ToSolidColorBrush();
        }
    }
}
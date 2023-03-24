using System.Windows.Markup;

namespace Acorisoft.FutureGL.Forest.Markup
{
    public class AdaptiveLevel1Extension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ThemeSystem.Instance
                              .Theme
                              .Colors[(int)ForestTheme.AdaptiveLevel1];
        }
    }
    
    public class AdaptiveLevel1BrushExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ThemeSystem.Instance
                              .Theme
                              .Colors[(int)ForestTheme.AdaptiveLevel1]
                              .ToSolidColorBrush();
        }
    }
}
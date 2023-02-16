using System.Windows;

namespace Acorisoft.FutureGL.Forest.Controls
{
    public class ForestTabControl : TabControl
    {
        static ForestTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestTabControl), new FrameworkPropertyMetadata(typeof(ForestTabControl)));
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ForestTabItem();
        }
    }
}
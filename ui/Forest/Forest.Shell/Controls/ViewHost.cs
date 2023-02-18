using System.Windows.Controls;

namespace Acorisoft.FutureGL.Forest.Controls
{
    public class ViewHost : Control
    {
        static ViewHost()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ViewHost), new FrameworkPropertyMetadata(typeof(ViewHost)));
        }
        
    }
}
using System.Windows;
using System.Windows.Media;
using Acorisoft.FutureGL.Forest.Styles;

namespace Acorisoft.FutureGL.Forest.Controls.Buttons
{
    public class ForestIconButton : ForestIconButtonBase
    {
        static ForestIconButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestIconButton), new FrameworkPropertyMetadata(typeof(ForestIconButton)));
        }
        
        protected override void GetTemplateChildOverride(ITemplatePartFinder finder)
        {
        }

        
    }
}
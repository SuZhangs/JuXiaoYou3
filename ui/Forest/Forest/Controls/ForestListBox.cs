namespace Acorisoft.FutureGL.Forest.Controls
{
    public class ForestListBox : ListBox
    {
        static ForestListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestListBox), new FrameworkPropertyMetadata(typeof(ForestListBox)));
        }
        
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ForestListBoxItem();
        }
    }
}
namespace Acorisoft.FutureGL.Forest.Controls.Selectors
{
    public abstract class ForestListBoxBase : ListBox
    {
        static ForestListBoxBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestListBoxBase), new FrameworkPropertyMetadata(typeof(ForestListBoxBase)));
        }
    }
}
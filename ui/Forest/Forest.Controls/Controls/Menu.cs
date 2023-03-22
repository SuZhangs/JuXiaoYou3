namespace Acorisoft.FutureGL.Forest.Controls
{
    public class Menu : ForestMenuBase
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MenuItem();
        }
    }

    public class ContextMenu : ForestContextMenuBase
    {
        
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MenuItem();
        }
    }

    public class MenuItem : ForestMenuItemBase
    {
        
    }
}
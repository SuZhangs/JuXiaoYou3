using System.Windows.Shapes;

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
        private const string PART_BdName         = "PART_Bd";
        private const string PART_ContentName    = "PART_Content";
        private const string PART_IconName       = "PART_Icon";
        private const string PART_SymbolIconName = "SymbolIcon";

        private Border           _bd;
        private MenuItemRole     _oldRole;
        private ContentPresenter _content;
        private Path             _icon;
        private Storyboard       _storyboard;

        private SolidColorBrush _backgroundBrush;
        private SolidColorBrush _foregroundBrush;
        private SolidColorBrush _backgroundHighlight1Brush;
        private SolidColorBrush _backgroundHighlight2Brush;
        private SolidColorBrush _foregroundHighlightBrush;
        private SolidColorBrush _backgroundDisabledBrush;
        private SolidColorBrush _foregroundDisabledBrush;


        protected override void StopAnimation()
        {
        }

        protected override void SetForeground(Brush brush)
        {
        }

        protected override void OnInvalidateState()
        {
        }

        protected override void GoToNormalState(HighlightColorPalette palette, ForestThemeSystem theme)
        {
            var newRole = Role;

            if (newRole != _oldRole)
            {
                InvalidateState();
                _oldRole = newRole;
            }
        }

        protected override void GoToHighlight1State(Duration duration, HighlightColorPalette palette, ForestThemeSystem theme)
        {
            
        }

        protected override void GoToHighlight2State(Duration duration, HighlightColorPalette palette, ForestThemeSystem theme)
        {
            
        }

        protected override void GoToDisableState(HighlightColorPalette palette, ForestThemeSystem theme)
        {
            
        }

        protected override void GetTemplateChildOverride(ITemplatePartFinder finder)
        {
            
        }
    }
}
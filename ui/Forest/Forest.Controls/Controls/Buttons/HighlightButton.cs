using System.Windows.Documents;
using System.Windows.Media;

using Acorisoft.FutureGL.Forest.Styles;

namespace Acorisoft.FutureGL.Forest.Controls.Buttons
{
    public class HighlightButton : ForestButtonBase
    {
        static HighlightButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HighlightButton), new FrameworkPropertyMetadata(typeof(HighlightButton)));
        }

        private const string PART_BdName      = "PART_Bd";
        private const string PART_ContentName = "PART_Content";

        private Border           _bd;
        private ContentPresenter _content;

        protected override void GetTemplateChildOverride(ITemplatePartFinder finder)
        {
            finder.Find<Border>(PART_BdName, x => _bd                     = x)
                  .Find<ContentPresenter>(PART_ContentName, x => _content = x);
        }
    }
}
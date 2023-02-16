using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Acorisoft.FutureGL.Forest.Enums;
using Acorisoft.FutureGL.Forest.Styles;
using Acorisoft.FutureGL.Forest.Styles.Animations;
using VisualState = Acorisoft.FutureGL.Forest.Enums.VisualState;

namespace Acorisoft.FutureGL.Forest.Controls.Buttons
{
    public class HighlightButton : ForestButton
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

        protected override void BuildAnimation()
        {
            var theme = ThemeSystem.Instance.Theme;
            var palette = Palette;

            var nsColor = palette switch
            {
                HighlightColorPalette.HighlightPalette2 => theme.Colors[(int)ForestTheme.HighlightB3],
                HighlightColorPalette.HighlightPalette3 => theme.Colors[(int)ForestTheme.HighlightC3],
                _ => theme.Colors[(int)ForestTheme.HighlightA3],
            };
            
            var h1Color = palette switch
            {
                HighlightColorPalette.HighlightPalette2 => theme.Colors[(int)ForestTheme.HighlightB4],
                HighlightColorPalette.HighlightPalette3 => theme.Colors[(int)ForestTheme.HighlightC4],
                _ => theme.Colors[(int)ForestTheme.HighlightA4],
            };
            
            var h2Color = palette switch
            {
                HighlightColorPalette.HighlightPalette2 => theme.Colors[(int)ForestTheme.HighlightB5],
                HighlightColorPalette.HighlightPalette3 => theme.Colors[(int)ForestTheme.HighlightC5],
                _ => theme.Colors[(int)ForestTheme.HighlightA5],
            };
            var nfColor = theme.Colors[(int)ForestTheme.ForegroundInHighlight];

            // 状态驱动的动画
            //
            // 应该避免这种写法
            var animatorBuilder = StateDrivenAnimation();

            animatorBuilder.TargetAndDefault(_bd) // 构造默认视觉效果
                           .Set(BackgroundProperty, nsColor.ToSolidColorBrush())
                           .Finish();

            animatorBuilder.TargetAndDefault(_content)
                           .Set(TextElement.ForegroundProperty, nfColor.ToSolidColorBrush())
                           .Finish();

            animatorBuilder.Target(_bd) // 构造Border视觉效果
                           .Color(theme.Duration.Medium, BackgroundProperty, SolidColorBrush.ColorProperty)
                           .Next(VisualState.Normal, nsColor)
                           .Next(VisualState.Highlight1, h1Color)
                           .Next(VisualState.Highlight2, h2Color)
                           .Next(VisualState.Inactive, theme.Colors[(int)ForestTheme.Background])
                           .Finish();

            Animator = animatorBuilder.Target(_content) // 构造ContentPresenter视觉效果
                                      .Foreground(theme.Duration.Samll)
                                      .Next(VisualState.Normal, nfColor)
                                      .Next(VisualState.Inactive, Colors.Gray)
                                      .FinalFinish();
        }
    }
}
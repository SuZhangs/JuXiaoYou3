using System.Windows.Documents;
using System.Windows.Media;

using Acorisoft.FutureGL.Forest.Styles;
using Acorisoft.FutureGL.Forest.Styles.Animations;

namespace Acorisoft.FutureGL.Forest.Controls.Buttons
{
    public class CallToAction : ForestButtonBase
    {
        static CallToAction()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CallToAction), new FrameworkPropertyMetadata(typeof(CallToAction)));
        }

        private const string           PART_BdName      = "PART_Bd";
        private const string           PART_ContentName = "PART_Content";
        private       Border           _bd;
        private       ContentPresenter _content;

        protected override void GetTemplateChildOverride(ITemplatePartFinder finder)
        {
            finder.Find<Border>(PART_BdName, x => _bd                     = x)
                  .Find<ContentPresenter>(PART_ContentName, x => _content = x);
        }

        protected override void BuildAnimation()
        {
            var theme = ThemeSystem.Instance.Theme;
            var palette = Palette;
            
            // TODO: wait fixed

            var nsColor = theme.GetCallToActionColor(palette, 1);
            var h1Color = theme.GetCallToActionColor(palette, 2);
            var h2Color = theme.GetCallToActionColor(palette, 3);
            var nfColor = theme.Colors[(int)ForestTheme.ForegroundInHighlight];
            var ndColor = theme.GetCallToActionColor(palette, 4);

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
                           .Next(VisualState.Inactive, ndColor)
                           .Finish();

            Animator = animatorBuilder.Target(_content) // 构造ContentPresenter视觉效果
                                      .Foreground(theme.Duration.Samll)
                                      .Next(VisualState.Normal, nfColor)
                                      .Next(VisualState.Inactive, theme.Colors[(int)ForestTheme.ForegroundInActive])
                                      .FinalFinish();
        }
    }
}
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
                  .Find<ContentPresenter>(PART_ContentName, x => _content = x)
                  .Done(BuildAnimation);
        }

        protected override void BuildAnimation()
        {
            var theme = ThemeSystem.Instance.Theme;

            var nsColor = theme.Colors[(int)ForestTheme.HighlightA3];
            var h1Color = theme.Colors[(int)ForestTheme.HighlightA1];
            var h2Color = theme.Colors[(int)ForestTheme.HighlightA5];
            var nfColor = theme.Colors[(int)ForestTheme.Foreground];
            var nhColor = theme.Colors[(int)ForestTheme.ForegroundInHighlight];

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
                           .Color(new Duration(TimeSpan.FromMilliseconds(200)), BackgroundProperty, SolidColorBrush.ColorProperty)
                           .Next(VisualState.Normal, nsColor)
                           .Next(VisualState.Highlight1, h1Color)
                           .Next(VisualState.Highlight2, h2Color)
                           .Next(VisualState.Inactive, nsColor)
                           .Finish();

            Animator = animatorBuilder.Target(_content) // 构造ContentPresenter视觉效果
                                      .Foreground(new Duration(TimeSpan.FromMilliseconds(200)))
                                      .Next(VisualState.Normal, nfColor)
                                      .Next(VisualState.Highlight1, nhColor)
                                      .Next(VisualState.Highlight2, nhColor)
                                      .Next(VisualState.Inactive, Colors.Gray)
                                      .FinalFinish();
        }
    }
}
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

        private readonly ITemplatePartFinder _finder;
        private const    string              PART_BdName      = "PART_Bd";
        private const    string              PART_ContentName = "PART_Content";

        private Border           _bd;
        private ContentPresenter _content;

        public HighlightButton()
        {
            //
            _finder = GetTemplateChild()
                      .Find<Border>(PART_BdName, x => _bd                     = x)
                      .Find<ContentPresenter>(PART_ContentName, x => _content = x)
                      .Done(BuildAnimation);
            
            BuildState();
        }

        private void BuildState()
        {
            StateMachine.AddState(VisualState.Normal, VisualState.Highlight1, VisualStateTrigger.Next);
            StateMachine.AddState(VisualState.Highlight1, VisualState.Highlight2, VisualStateTrigger.Next);
            StateMachine.AddState(VisualState.Highlight2, VisualState.Normal, VisualStateTrigger.Next, false);
            StateMachine.AddState(VisualState.Highlight1, VisualState.Highlight1, VisualStateTrigger.Disabled);
            StateMachine.AddState(VisualState.Highlight2, VisualState.Highlight1, VisualStateTrigger.Disabled);
            StateMachine.AddState(VisualState.Normal, VisualState.Inactive, VisualStateTrigger.Disabled);
        }

        private void BuildAnimation()
        {
            var theme = Theme ??= ThemeSystem.Instance.Theme;
            
            var nsColor = theme.Colors[(int)ForestTheme.HighlightA3];
            var h1Color = theme.Colors[(int)ForestTheme.HighlightA1];
            var h2Color = theme.Colors[(int)ForestTheme.HighlightA5];
            var nfColor = theme.Colors[(int)ForestTheme.Foreground];
            var nhColor = theme.Colors[(int)ForestTheme.ForegroundInHighlight];

            // 状态驱动的动画
            base.StateDrivenAnimation()
                .TargetAndDefault(_bd) // 构造默认视觉效果
                .Set(BackgroundProperty, nsColor.ToSolidColorBrush())
                .Continue(_content)
                .Set(TextElement.ForegroundProperty, nfColor.ToSolidColorBrush())
                .Target(_bd) // 构造Border视觉效果
                .Color(new Duration(TimeSpan.FromMilliseconds(200)), BackgroundProperty, SolidColorBrush.ColorProperty)
                .Next(VisualState.Normal, nsColor)
                .Next(VisualState.Highlight1, h1Color)
                .Next(VisualState.Highlight2, h2Color)
                .Next(VisualState.Inactive, nsColor)
                .NextElement(_content) // 构造ContentPresenter视觉效果
                .Color(new Duration(TimeSpan.FromMilliseconds(200)), TextElement.ForegroundProperty, SolidColorBrush.ColorProperty)
                .Next(VisualState.Normal, nfColor)
                .Next(VisualState.Highlight1, nhColor)
                .Next(VisualState.Highlight2, nhColor)
                .Next(VisualState.Inactive, Colors.Gray)
                .Finish();

            // IStateDrivenAnimationBuilder
            // IStateDrivenTargetBuilder
            // IStateDrivenPropertyAnimationBuilder
        }


        public override void OnApplyTemplate()
        {
            _finder.Dispose();
            StateMachine.NextState();
            base.OnApplyTemplate();
        }
    }
}
using System.Windows.Media;
using System.Windows.Documents;
using System.Windows.Media.Animation;

using Acorisoft.FutureGL.Forest.Styles;

namespace Acorisoft.FutureGL.Forest.Controls
{
    public class ForestTextBox : ForestTextBoxBase
    {
        static ForestTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestTextBox), new FrameworkPropertyMetadata(typeof(ForestTextBox)));
        }
        
        private Storyboard _storyboard;

        public ForestTextBox()
        {
            StateMachine.StateChangedHandler =  HandleStateChanged;
        }

        private void HandleStateChanged(bool init, VisualState last, VisualState now, VisualStateTrigger value)
        {
            var palette = Palette;
            var theme   = ThemeSystem.Instance.Theme;

            // 正常的背景颜色
            var background          = theme.Colors[(int)ForestTheme.Background];
            var foreground          = theme.Colors[(int)ForestTheme.Foreground];
            var highlightBackground = theme.GetHighlightColor(palette, 3);
            var activeBackground    = theme.GetHighlightColor(palette, 1);
            var disabledBackground  = theme.Colors[(int)ForestTheme.BackgroundInactive];
            var highlightForeground = theme.Colors[(int)ForestTheme.ForegroundInHighlight];
            var disabledForeground  = theme.Colors[(int)ForestTheme.ForegroundInActive];

            // Stop Animation
            _storyboard?.Stop(PART_Bd);

            if (!init)
            {
                HandleNormalState(
                    ref background,
                    ref disabledBackground,
                    ref disabledForeground,
                    ref foreground);
            }
            else
            {
                if (now == VisualState.Highlight1)
                {
                    HandleHighlight1State(
                        theme.Duration.Samll,
                        ref background,
                        ref activeBackground,
                        ref highlightForeground);
                }
                else if (now == VisualState.Highlight2)
                {
                    HandleHighlight2State(
                        theme.Duration.Medium,
                        ref activeBackground,
                        ref highlightBackground,
                        ref highlightForeground);
                }
                else if (now == VisualState.Normal)
                {
                    HandleNormalState(
                        ref background,
                        ref disabledBackground,
                        ref disabledForeground,
                        ref foreground);
                }
                else if (now == VisualState.Inactive)
                {
                    HandleDisabledState(ref disabledForeground, ref disabledForeground);
                }
            }
        }

        private void HandleDisabledState(ref Color background, ref Color foreground)
        {
            //
            // 设置背景颜色
            PART_Bd.Background = background.ToSolidColorBrush();
            PART_Bd.Effect     = null;

            // 设置文本颜色
            SetForeground(ref foreground);
        }

        private void SetForeground(ref Color highlightForeground)
        {
            var foreground = highlightForeground.ToSolidColorBrush();
            Foreground = foreground;
            PART_Content.SetValue(TextElement.ForegroundProperty, foreground);
        }

        private void HandleNormalState(ref Color background, ref Color disabledBackground, ref Color disabledForeground, ref Color foreground)
        {
            if (!IsEnabled)
            {
                HandleDisabledState(ref disabledBackground, ref disabledForeground);
                return;
            }

            //
            // 设置背景颜色
            PART_Bd.Background = background.ToSolidColorBrush();
            PART_Bd.Effect     = null;

            // 设置文本颜色
            SetForeground(ref foreground);
        }

        private void HandleHighlight1State(Duration duration, ref Color background, ref Color highlightBackground, ref Color highlightForeground)
        {
            //
            // Opacity 动画

            var backgroundAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = Colors.Gray,
                To       = highlightBackground,
            };

            Storyboard.SetTarget(backgroundAnimation, PART_Bd);
            Storyboard.SetTargetProperty(backgroundAnimation, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));

            _storyboard = new Storyboard
            {
                Children = new TimelineCollection { backgroundAnimation }
            };

            //
            // 开始动画
            PART_Bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);

            // 设置文本颜色
            SetForeground(ref highlightForeground);
        }

        private void HandleHighlight2State(Duration duration, ref Color background, ref Color highlightBackground, ref Color highlightForeground)
        {
            //
            // Opacity 动画
            // var OpacityAnimation = new DoubleAnimation()
            // {
            //     Duration = duration,
            //     From     = 0.5,
            //     To       = 1,
            // };

            var backgroundAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = background,
                To       = highlightBackground,
            };

            // Storyboard.SetTarget(OpacityAnimation, _bd);
            Storyboard.SetTarget(backgroundAnimation, PART_Bd);
            // Storyboard.SetTargetProperty(OpacityAnimation, new PropertyPath(OpacityProperty));
            Storyboard.SetTargetProperty(backgroundAnimation, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));

            _storyboard = new Storyboard
            {
                Children = new TimelineCollection
                {
                    /*OpacityAnimation,*/ backgroundAnimation
                }
            };

            //
            // 开始动画
            PART_Bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);

            // 设置文本颜色
            SelectionBrush = ThemeSystem.Instance.Theme.Colors[(int)ForestTheme.HighlightA2].ToSolidColorBrush();
            SetForeground(ref highlightForeground);
        }

    }
}
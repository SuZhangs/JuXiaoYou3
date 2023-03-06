using System.Windows.Media;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

using Acorisoft.FutureGL.Forest.Styles;

namespace Acorisoft.FutureGL.Forest.Controls
{
    public class HighlightTextBox : ForestTextBoxBase
    {
        static HighlightTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HighlightTextBox), new FrameworkPropertyMetadata(typeof(HighlightTextBox)));
            PaletteProperty.AddOwner(typeof(HighlightTextBox))
                           .OverrideMetadata(typeof(HighlightTextBox), new PropertyMetadata(OnPaletteChanged));
        }

        private static void OnPaletteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((HighlightTextBox)d).InvalidateState();
        }

        private Storyboard _storyboard;

        private SolidColorBrush _backgroundBrush;
        private SolidColorBrush _foregroundBrush;
        private SolidColorBrush _backgroundHighlight1Brush;
        private SolidColorBrush _backgroundHighlight2Brush;
        private SolidColorBrush _foregroundHighlightBrush;
        private SolidColorBrush _backgroundDisabledBrush;
        private SolidColorBrush _foregroundDisabledBrush;
        

        public HighlightTextBox()
        {
            StateMachine.StateChangedHandler =  HandleStateChanged;
        }

        private void InvalidateState()
        {
            _backgroundBrush           = null;
            _foregroundBrush           = null;
            _backgroundHighlight1Brush = null;
            _foregroundHighlightBrush = null;
            _backgroundHighlight2Brush = null;
            _backgroundDisabledBrush   = null;
            _foregroundDisabledBrush   = null;
            InvalidateVisual();
        }

        private void HandleStateChanged(bool init, VisualState last, VisualState now, VisualStateTrigger value)
        {
            var theme   = ThemeSystem.Instance.Theme;

            // Stop Animation
            _storyboard?.Stop(PART_Bd);

            if (!init)
            {
                HandleNormalState();
            }
            else
            {
                if (now == VisualState.Highlight1)
                {
                    HandleHighlight1State(theme.Duration.Medium);
                }
                else if (now == VisualState.Highlight2)
                {
                    HandleHighlight2State(theme.Duration.Medium);
                }
                else if (now == VisualState.Normal)
                {
                    HandleNormalState();
                }
                else if (now == VisualState.Inactive)
                {
                    HandleDisabledState();
                }
            }
        }

        private void HandleDisabledState()
        {
            if (_foregroundDisabledBrush is null || 
                _backgroundDisabledBrush is null)
            {
                var theme = ThemeSystem.Instance.Theme;
                _backgroundDisabledBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundInactive]);
                _foregroundDisabledBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInActive]);
            }
            
            //
            // 设置背景颜色
            PART_Bd.Background = _backgroundDisabledBrush;
            PART_Bd.Effect     = null;

            // 设置文本颜色
            SetForeground(_foregroundDisabledBrush);
        }

        private void SetForeground(SolidColorBrush brush)
        {
            PART_Content.SetValue(TextElement.ForegroundProperty, brush);
        }

        private void HandleNormalState()
        {
            if (_foregroundDisabledBrush is null || 
                _backgroundDisabledBrush is null)
            {
                var theme = ThemeSystem.Instance.Theme;
                _backgroundBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.Background]);
                _foregroundBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.Foreground]);
            }
            
            if (!IsEnabled || IsReadOnly)
            {
                HandleDisabledState();
                return;
            }

            //
            // 设置背景颜色
            PART_Bd.Background = _backgroundBrush;
            PART_Bd.Effect     = null;

            // 设置文本颜色
            SetForeground(_foregroundBrush);
        }

        private void HandleHighlight1State(Duration duration)
        {
            //
            // Opacity 动画
            if (_backgroundBrush is null ||
                _backgroundHighlight1Brush is null ||
                _foregroundHighlightBrush is null)
            {
                var theme = ThemeSystem.Instance.Theme;
                _backgroundBrush           ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.Background]);
                _backgroundHighlight1Brush ??= new SolidColorBrush(theme.GetHighlightColor(Palette, 1));
                _foregroundHighlightBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInHighlight]);
            }


            var backgroundAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _backgroundBrush.Color,
                To       = _backgroundHighlight1Brush.Color,
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
            SetForeground(_foregroundHighlightBrush);
        }

        private void HandleHighlight2State(Duration duration)
        {
            //
            // Opacity 动画
            // var OpacityAnimation = new DoubleAnimation()
            // {
            //     Duration = duration,
            //     From     = 0.5,
            //     To       = 1,
            // };
            if (_backgroundBrush is null ||
                _backgroundHighlight2Brush is null ||
                _foregroundHighlightBrush is null)
            {
                var theme = ThemeSystem.Instance.Theme;
                _backgroundBrush           ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.Background]);
                _backgroundHighlight1Brush ??= new SolidColorBrush(theme.GetHighlightColor(Palette, 1));
                _backgroundHighlight2Brush ??= new SolidColorBrush(theme.GetHighlightColor(Palette, 3));
                _foregroundHighlightBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInHighlight]);
            }
            
            var backgroundAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _backgroundHighlight1Brush.Color,
                To       = _backgroundHighlight2Brush.Color,
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

            // 创建阴影
            PART_Bd.Effect = BuildHighlightShadowEffect(_backgroundHighlight2Brush.Color);

            // 设置文本颜色
            SelectionBrush = ThemeSystem.Instance.Theme.Colors[(int)ForestTheme.HighlightA2].ToSolidColorBrush();
            SetForeground(_foregroundHighlightBrush);
        }

        private static DropShadowEffect BuildHighlightShadowEffect(Color color)
        {
            // <DropShadowEffect x:Key = "Shadow.A3" Direction = "270" ShadowDepth = "4.5" BlurRadius = "14" Opacity = "0.42" Color = "{DynamicResource AccentA3}" />
            return new DropShadowEffect
            {
                Color       = color,
                ShadowDepth = 4.5d,
                BlurRadius  = 14,
                Opacity     = 0.42d,
                Direction   = 270
            };
        }
        
    }
}
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using Acorisoft.FutureGL.Forest.Styles;

namespace Acorisoft.FutureGL.Forest.Controls.TabControls
{
    public class ToolControl : ForestTabControl
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ToolItem();
        }
    }

    public class ToolItem : ForestTabItemBase
    {
        
        private const string PART_BdName      = "PART_Bd";
        private const string PART_ContentName = "PART_Content";
        private const string PART_IconName    = "PART_Icon";
        private const string PART_LineName    = "PART_Line";


        private Storyboard       _storyboard;
        private Border           _bd;
        private Border           _line;
        private ContentPresenter _content;
        private Path             _icon;

        private SolidColorBrush _background;
        private SolidColorBrush _foreground;
        private SolidColorBrush _highlight;
        private SolidColorBrush _highlight2;
        private SolidColorBrush _disabled;
        
        protected override void OnStateChanged(bool init, VisualState now)
        {
            var palette = Palette;
            var theme = ThemeSystem.Instance.Theme;

            _background = theme.Colors[(int)ForestTheme.Background].ToSolidColorBrush();
            _foreground = theme.Colors[(int)ForestTheme.Foreground].ToSolidColorBrush();
            _highlight  = theme.GetHighlightColor(palette, 1).ToSolidColorBrush();
            _highlight2 = theme.GetHighlightColor(palette, 3).ToSolidColorBrush();
            _disabled   = theme.Colors[(int)ForestTheme.BackgroundInactive].ToSolidColorBrush();

            // Stop Animation
            _storyboard?.Stop(_bd);

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

        protected override void GetTemplateChildOverride(ITemplatePartFinder finder)
        {
            finder.Find<Border>(PART_BdName, x => _bd                     = x)
                  .Find<Border>(PART_LineName, x => _line                 = x)
                  .Find<Path>(PART_IconName, x => _icon                   = x)
                  .Find<ContentPresenter>(PART_ContentName, x => _content = x);
        }


        private void HandleDisabledState()
        {
            //
            // 设置背景颜色
            _bd.Background    = _disabled;
            _line.BorderBrush = _disabled;

            // 设置文本颜色
            SetForeground();
        }

        private void SetForeground()
        {
            _content.SetValue(TextElement.ForegroundProperty, _foreground);

            if (IsFilled)
            {
                _icon.Fill = _foreground;
            }
            else
            {
                _icon.Stroke          = _foreground;
                _icon.StrokeThickness = 1d;
            }
        }

        private void HandleNormalState()
        {
            if (!IsEnabled)
            {
                HandleDisabledState();
                return;
            }
            
            if (IsSelected)
            {
                //
                // 设置背景颜色
                _bd.Background    = _highlight2;
                _line.BorderBrush = _highlight2;
            }
            else
            {
                //
                // 设置背景颜色
                _bd.Background    = _background;
                _line.BorderBrush = _background;

            }
            
            // 设置文本颜色
            SetForeground();
        }

        private void HandleHighlight1State(Duration duration)
        {
            //
            // Opacity 动画

            var backgroundAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _background.Color,
                To       = _highlight.Color,
            };
            var backgroundAnimation2 = new ColorAnimation
            {
                Duration = duration,
                From     = _background.Color,
                To       = _highlight.Color,
            };

            Storyboard.SetTarget(backgroundAnimation, _bd);
            Storyboard.SetTarget(backgroundAnimation2, _line);
            Storyboard.SetTargetProperty(backgroundAnimation2, new PropertyPath("(Border.Background).(SolidColorBrush.Color)"));
            Storyboard.SetTargetProperty(backgroundAnimation, new PropertyPath("(Border.Background).(SolidColorBrush.Color)"));

            _storyboard = new Storyboard
            {
                Children = new TimelineCollection { backgroundAnimation }
            };

            //
            // 开始动画
            _bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);
            _line.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);

            // 设置文本颜色
             SetForeground();
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

            var backgroundAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _highlight.Color,
                To       = _highlight2.Color,
            };
            
            var backgroundAnimation2 = new ColorAnimation
            {
                Duration = duration,
                From     = _highlight.Color,
                To       = _highlight2.Color,
            };

            // Storyboard.SetTarget(OpacityAnimation, _bd);
            Storyboard.SetTarget(backgroundAnimation, _bd);
            Storyboard.SetTarget(backgroundAnimation2, _line);
            // Storyboard.SetTargetProperty(OpacityAnimation, new PropertyPath(OpacityProperty));
            Storyboard.SetTargetProperty(backgroundAnimation, new PropertyPath("(Border.Background).(SolidColorBrush.Color)"));
            Storyboard.SetTargetProperty(backgroundAnimation2, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));

            _storyboard = new Storyboard
            {
                Children = new TimelineCollection { /*OpacityAnimation,*/ backgroundAnimation }
            };

            //
            // 开始动画
            _bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);
            _line.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);

            // 设置文本颜色
            SetForeground();
        }
    }
}
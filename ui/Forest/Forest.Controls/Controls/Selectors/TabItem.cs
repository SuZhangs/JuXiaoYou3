using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.Forest.Styles;
using Acorisoft.FutureGL.Forest.Styles.Animations;

namespace Acorisoft.FutureGL.Forest.Controls.Selectors
{
    public class TabItem : ForestTabItemBase
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius),
            typeof(double),
            typeof(TabItem),
            new PropertyMetadata(default(double)));
        
        private const string PART_BdName        = "PART_Bd";
        private const string PART_ContentName   = "PART_Content";
        private const string PART_IconName      = "PART_Icon";
        private const string PART_DecoratorName = "PART_Decorator";


        private Storyboard       _storyboard;
        private Border           _bd;
        private Border           _decorator;
        private ContentPresenter _content;
        private Path             _icon;
        private Dock             _placement;

        public TabItem()
        {
            var controller = ItemsControl.GetItemsOwner(this) as TabControl;
            var placement = controller?.TabStripPlacement ?? Dock.Top;
            
            
        }
        
        protected override void OnStateChanged(bool init, VisualState now)
        {
            var palette = Palette;
            var theme = ThemeSystem.Instance.Theme;

            // 正常的背景颜色
            var background = theme.Colors[(int)ForestTheme.Background];
            var foreground = theme.Colors[(int)ForestTheme.Foreground];
            var highlightBackground = theme.GetHighlightColor(palette, 3);
            var activeBackground = theme.GetHighlightColor(palette, 1);
            var highlightForeground = theme.Colors[(int)ForestTheme.ForegroundInHighlight];
            var disabledForeground = theme.Colors[(int)ForestTheme.ForegroundInActive];

            // Stop Animation
            _storyboard?.Stop(_bd);

            if (!init)
            {
                HandleNormalState(
                    ref background,
                    ref disabledForeground,
                    ref foreground,
                    ref highlightBackground,
                    ref highlightForeground);
            }
            else
            {
                if (now == VisualState.Highlight1)
                {
                    HandleHighlight1State(
                        theme.Duration.Medium, 
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
                        ref disabledForeground,
                        ref foreground,
                        ref highlightBackground,
                        ref highlightForeground);
                }
                else if (now == VisualState.Inactive)
                {
                    HandleDisabledState(ref background, ref disabledForeground);
                }
            }
        }

        protected override void GetTemplateChildOverride(ITemplatePartFinder finder)
        {
            finder.Find<Border>(PART_BdName, x => _bd                   = x)
                .Find<Path>(PART_IconName, x => _icon                   = x)
                .Find<ContentPresenter>(PART_ContentName, x => _content = x);
        }


        private void HandleDisabledState(ref Color background,ref Color foreground)
        {
            //
            // 设置背景颜色
            _bd.Background = background.ToSolidColorBrush();
            _bd.Effect     = null;

            // 设置文本颜色
            SetForeground(ref foreground);
        }

        private void SetForeground(ref Color highlightForeground)
        {
            var foreground = highlightForeground.ToSolidColorBrush();
            _content.SetValue(TextElement.ForegroundProperty, foreground);

            if (IsFilled)
            {
                _icon.StrokeThickness = 0d;
                _icon.Fill = foreground;
            }
            else
            {
                _icon.Stroke          = foreground;
                _icon.StrokeThickness = 1d;
            }
        }

        private void HandleNormalState(ref Color background, ref Color disabledForeground, ref Color foreground,
            ref Color highlightBackground, ref Color highlightForeground)
        {
            if (!IsEnabled)
            {
                HandleDisabledState(ref background, ref disabledForeground);
                return;
            }
            if (IsSelected)
            {
                //
                // 设置背景颜色
                _bd.Background = highlightBackground.ToSolidColorBrush();

                // 创建阴影
                _bd.Effect = BuildHighlightShadowEffect(ref highlightBackground);

                // 设置文本颜色
                SetForeground(ref highlightForeground);
            }
            else
            {
                //
                // 设置背景颜色
                _bd.Background = background.ToSolidColorBrush();
                _bd.Effect     = null;

                // 设置文本颜色
                SetForeground(ref foreground);
            }
        }

        private void HandleHighlight1State(Duration duration, ref Color background,ref Color highlightBackground, ref Color highlightForeground)
        {
            //
            // Opacity 动画

            var backgroundAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = background,
                To       = highlightBackground,
            };

            Storyboard.SetTarget(backgroundAnimation, _bd);
            Storyboard.SetTargetProperty(backgroundAnimation, new PropertyPath("(Border.Background).(SolidColorBrush.Color)"));

            _storyboard = new Storyboard
            {
                Children = new TimelineCollection { backgroundAnimation }
            };

            //
            // 开始动画
            _bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);

            // 设置文本颜色
            // SetForeground(ref highlightForeground);
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
            Storyboard.SetTarget(backgroundAnimation, _bd);
            // Storyboard.SetTargetProperty(OpacityAnimation, new PropertyPath(OpacityProperty));
            Storyboard.SetTargetProperty(backgroundAnimation, new PropertyPath("(Border.Background).(SolidColorBrush.Color)"));

            _storyboard = new Storyboard
            {
                Children = new TimelineCollection { /*OpacityAnimation,*/ backgroundAnimation }
            };

            //
            // 开始动画
            _bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);

            // 创建阴影
            _bd.Effect = BuildHighlightShadowEffect(ref highlightBackground);

            // 设置文本颜色
            SetForeground(ref highlightForeground);
        }

        private static DropShadowEffect BuildHighlightShadowEffect(ref Color color)
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

        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
    }
}
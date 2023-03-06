using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using Acorisoft.FutureGL.Forest.Styles;

namespace Acorisoft.FutureGL.Forest.Controls.Buttons
{
    /// <summary>
    /// 这是一个特殊用途的按钮
    /// </summary>
    public class DialogButton : ForestButtonBase
    {
        public static readonly DependencyProperty SensitiveCaseProperty = DependencyProperty.Register(
            nameof(SensitiveCase),
            typeof(bool),
            typeof(DialogButton),
            new PropertyMetadata(Boxing.False));

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            nameof(Icon),
            typeof(Geometry),
            typeof(DialogButton),
            new PropertyMetadata(default(Geometry)));


        public static readonly DependencyProperty IsFilledProperty = DependencyProperty.Register(
            nameof(IsFilled),
            typeof(bool),
            typeof(DialogButton),
            new PropertyMetadata(Boxing.False));


        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(
            nameof(IconSize),
            typeof(double),
            typeof(DialogButton),
            new PropertyMetadata(17d));

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(DialogButton),
            new PropertyMetadata(new CornerRadius(8)));


        private const string PART_BdName      = "PART_Bd";
        private const string PART_ContentName = "PART_Content";
        private const string PART_IconName    = "PART_Icon";

        private Border           _bd;
        private ContentPresenter _content;
        private Path             _icon;
        private Storyboard       _storyboard;

        public DialogButton()
        {
            StateMachine.StateChangedHandler = OnStateChanged;
        }


        private void OnStateChanged(bool init, VisualState last, VisualState now, VisualStateTrigger value)
        {
            var palette = Palette;
            var theme   = ThemeSystem.Instance.Theme;

            // 正常的背景颜色
            var background          = theme.Colors[(int)ForestTheme.Background];
            var foreground          = theme.Colors[(int)ForestTheme.Foreground];
            var highlightBackground = theme.GetHighlightColor(palette, 3);
            var activeBackground    = theme.GetHighlightColor(palette, 1);
            var disabledBackground  = theme.Colors[(int)ForestTheme.BackgroundInactive];;
            var highlightForeground = theme.Colors[(int)ForestTheme.ForegroundInHighlight];
            var disabledForeground  = theme.Colors[(int)ForestTheme.ForegroundInActive];

            // Stop Animation
            _storyboard?.Stop(_bd);

            if (!init)
            {
                HandleNormalState(
                    ref background,
                    ref foreground,
                    ref disabledBackground,
                    ref disabledForeground);
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
            _bd.Background = background.ToSolidColorBrush();
            _bd.Effect     = null;

            // 设置文本颜色
            SetForeground(ref foreground);
        }

        private void SetForeground(ref Color highlightForeground)
        {
            var foreground = highlightForeground.ToSolidColorBrush();
            _content.SetValue(TextElement.ForegroundProperty, foreground);
        }

        private void HandleNormalState(ref Color background, ref Color foreground, ref Color disabledBackground, ref Color disabledForeground)
        {
            if (!IsEnabled)
            {
                HandleDisabledState(ref disabledBackground, ref disabledForeground);
                return;
            }

            //
            // 设置背景颜色
            _bd.Background = background.ToSolidColorBrush();
            _bd.Effect     = null;

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
                From     = background,
                To       = highlightBackground,
            };

            Storyboard.SetTarget(backgroundAnimation, _bd);
            Storyboard.SetTargetProperty(backgroundAnimation, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));

            _storyboard = new Storyboard
            {
                Children = new TimelineCollection { backgroundAnimation }
            };

            //
            // 开始动画
            _bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);

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
            Storyboard.SetTarget(backgroundAnimation, _bd);
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
            _bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);

            // 创建阴影

            // 设置文本颜色
            // SelectionBrush = ThemeSystem.Instance.Theme.Colors[(int)ForestTheme.HighlightA2].ToSolidColorBrush();
            SetForeground(ref highlightForeground);
        }


        protected override void GetTemplateChildOverride(ITemplatePartFinder finder)
        {
            finder.Find<Border>(PART_BdName, x => _bd                     = x)
                  .Find<ContentPresenter>(PART_ContentName, x => _content = x)
                  .Find<Path>(PART_IconName, x => _icon                   = x);
        }


        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }

        public bool IsFilled
        {
            get => (bool)GetValue(IsFilledProperty);
            set => SetValue(IsFilledProperty, Boxing.Box(value));
        }

        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public bool SensitiveCase
        {
            get => (bool)GetValue(SensitiveCaseProperty);
            set => SetValue(SensitiveCaseProperty, Boxing.Box(value));
        }
    }
}
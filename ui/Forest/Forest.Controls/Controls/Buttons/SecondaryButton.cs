using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Acorisoft.FutureGL.Forest.Styles;
using Acorisoft.FutureGL.Forest.Styles.Animations;

namespace Acorisoft.FutureGL.Forest.Controls.Buttons
{
    public class SecondaryButton: ForestButtonBase
    {
        private const string PART_BdName      = "PART_Bd";
        private const string PART_ContentName = "PART_Content";

        private Border           _bd;
        private ContentPresenter _content;
        private Storyboard       _storyboard;

        private SolidColorBrush _backgroundBrush;
        private SolidColorBrush _borderBrush;
        private SolidColorBrush _foregroundBrush;
        private SolidColorBrush _backgroundHighlight1Brush;
        private SolidColorBrush _backgroundHighlight2Brush;
        private SolidColorBrush _foregroundHighlightBrush;

        public SecondaryButton()
        {
            StateMachine.StateChangedHandler = OnStateChanged;
        }

        private void InvalidateState()
        {
            _backgroundBrush           = null;
            _foregroundBrush           = null;
            _backgroundHighlight1Brush = null;
            _foregroundHighlightBrush = null;
            _backgroundHighlight2Brush = null;
            InvalidateVisual();
        }

        private void OnStateChanged(bool init, VisualState last, VisualState now, VisualStateTrigger value)
        {
            var theme = ThemeSystem.Instance.Theme;

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

        private void SetForeground(Brush foreground)
        {
            _content.SetValue(TextElement.ForegroundProperty, foreground);
        }

        private void HandleNormalState()
        {
            if (!IsEnabled)
            {
                HandleDisabledState();
                return;
            }

            var palette = Palette;               
            var theme = ThemeSystem.Instance.Theme;

            _backgroundBrush ??= theme.GetHighlightColor(palette, 1).ToSolidColorBrush();
            _borderBrush     ??= theme.GetHighlightColor(palette, 3).ToSolidColorBrush();
            _foregroundBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.Foreground]);

            _bd.Background  = _backgroundBrush;
            _bd.BorderBrush = _borderBrush;
            SetForeground(_foregroundBrush);
        }

        private void HandleHighlight1State(Duration duration)
        {
            //
            // Opacity 动画
            var palette = Palette;               
            var theme   = ThemeSystem.Instance.Theme;

            _foregroundHighlightBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInHighlight]);
            _backgroundHighlight1Brush ??= new SolidColorBrush(theme.GetHighlightColor(palette, 3));

            // 白底变特殊色
            // 高亮色变白色
            var backgroundAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _backgroundBrush.Color,
                To       = _backgroundHighlight1Brush.Color,
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
            SetForeground(_foregroundHighlightBrush);
        }

        private void HandleHighlight2State(Duration duration)
        {
            //
            // Opacity 动画
            var palette = Palette;               
            var theme   = ThemeSystem.Instance.Theme;

            _foregroundHighlightBrush  ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInHighlight]);
            _backgroundHighlight2Brush ??= new SolidColorBrush(theme.GetHighlightColor(palette, 4));

            // 白底变特殊色
            // 高亮色变白色
            var backgroundAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _backgroundHighlight1Brush.Color,
                To       = _backgroundHighlight2Brush.Color,
            };
            
            var borderBrushAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _backgroundHighlight1Brush.Color,
                To       = _backgroundHighlight2Brush.Color
            };

            Storyboard.SetTarget(backgroundAnimation, _bd);
            Storyboard.SetTargetProperty(backgroundAnimation, new PropertyPath("(Border.Background).(SolidColorBrush.Color)"));
            Storyboard.SetTarget(borderBrushAnimation, _bd);
            Storyboard.SetTargetProperty(borderBrushAnimation, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));

            _storyboard = new Storyboard
            {
                Children = new TimelineCollection { backgroundAnimation , borderBrushAnimation}
            };

            //
            // 开始动画
            _bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);

            // 设置文本颜色
            SetForeground(_foregroundHighlightBrush);
        }
        
        private void HandleDisabledState()
        {    
            var theme   = ThemeSystem.Instance.Theme;

            _backgroundBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.SlateGrayDisabled]);
            _borderBrush     ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.SlateGrayDisabled]);
            _foregroundBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundInactive]);

            _bd.Background  = _backgroundBrush;
            _bd.BorderBrush = _borderBrush;
            SetForeground(_foregroundBrush);
        }

        protected override void GetTemplateChildOverride(ITemplatePartFinder finder)
        {
            finder.Find<Border>(PART_BdName, x => _bd                     = x)
                  .Find<ContentPresenter>(PART_ContentName, x => _content = x);
        }
    }
    
    public class SecondaryIconButton: ForestButtonBase
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            nameof(Icon),
            typeof(Geometry),
            typeof(SecondaryIconButton),
            new PropertyMetadata(default(Geometry)));


        public static readonly DependencyProperty IsFilledProperty = DependencyProperty.Register(
            nameof(IsFilled),
            typeof(bool),
            typeof(SecondaryIconButton),
            new PropertyMetadata(Boxing.False));


        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(
            nameof(IconSize),
            typeof(double),
            typeof(SecondaryIconButton),
            new PropertyMetadata(17d));


        private const string PART_BdName      = "PART_Bd";
        private const string PART_ContentName = "PART_Content";
        private const string PART_IconName    = "PART_Icon";

        private Border           _bd;
        private ContentPresenter _content;
        private Path             _icon;
        private Storyboard       _storyboard;

        private SolidColorBrush _backgroundBrush;
        private SolidColorBrush _borderBrush;
        private SolidColorBrush _foregroundBrush;
        private SolidColorBrush _backgroundHighlight1Brush;
        private SolidColorBrush _backgroundHighlight2Brush;
        private SolidColorBrush _foregroundHighlightBrush;

        public SecondaryIconButton()
        {
            StateMachine.StateChangedHandler = OnStateChanged;
        }

        private void InvalidateState()
        {
            _backgroundBrush           = null;
            _foregroundBrush           = null;
            _backgroundHighlight1Brush = null;
            _foregroundHighlightBrush = null;
            _backgroundHighlight2Brush = null;
            InvalidateVisual();
        }

        private void OnStateChanged(bool init, VisualState last, VisualState now, VisualStateTrigger value)
        {
            var theme = ThemeSystem.Instance.Theme;

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

        private void SetForeground(Brush foreground)
        {
            _content.SetValue(TextElement.ForegroundProperty, foreground);

            if (IsFilled)
            {
                _icon.StrokeThickness = 0;
                _icon.Fill            = foreground;
            }
            else
            {
                _icon.StrokeThickness = 2;
                _icon.Stroke          = foreground;
            }
        }

        private void HandleNormalState()
        {
            if (!IsEnabled)
            {
                HandleDisabledState();
                return;
            }

            var palette = Palette;               
            var theme = ThemeSystem.Instance.Theme;

            _backgroundBrush ??= theme.GetHighlightColor(palette, 1).ToSolidColorBrush();
            _borderBrush     ??= theme.GetHighlightColor(palette, 3).ToSolidColorBrush();
            _foregroundBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.Foreground]);

            _bd.Background  = _backgroundBrush;
            _bd.BorderBrush = _borderBrush;
            SetForeground(_foregroundBrush);
        }

        private void HandleHighlight1State(Duration duration)
        {
            //
            // Opacity 动画
            var palette = Palette;               
            var theme   = ThemeSystem.Instance.Theme;

            _foregroundHighlightBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInHighlight]);
            _backgroundHighlight1Brush ??= new SolidColorBrush(theme.GetHighlightColor(palette, 3));

            // 白底变特殊色
            // 高亮色变白色
            var backgroundAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _backgroundBrush.Color,
                To       = _backgroundHighlight1Brush.Color,
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
            SetForeground(_foregroundHighlightBrush);
        }

        private void HandleHighlight2State(Duration duration)
        {
            //
            // Opacity 动画
            var palette = Palette;               
            var theme   = ThemeSystem.Instance.Theme;

            _foregroundHighlightBrush  ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInHighlight]);
            _backgroundHighlight2Brush ??= new SolidColorBrush(theme.GetHighlightColor(palette, 4));

            // 白底变特殊色
            // 高亮色变白色
            var backgroundAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _backgroundHighlight1Brush.Color,
                To       = _backgroundHighlight2Brush.Color,
            };
            
            var borderBrushAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _backgroundHighlight1Brush.Color,
                To       = _backgroundHighlight2Brush.Color
            };

            Storyboard.SetTarget(backgroundAnimation, _bd);
            Storyboard.SetTargetProperty(backgroundAnimation, new PropertyPath("(Border.Background).(SolidColorBrush.Color)"));
            Storyboard.SetTarget(borderBrushAnimation, _bd);
            Storyboard.SetTargetProperty(borderBrushAnimation, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));

            _storyboard = new Storyboard
            {
                Children = new TimelineCollection { backgroundAnimation , borderBrushAnimation}
            };

            //
            // 开始动画
            _bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);

            // 设置文本颜色
            SetForeground(_foregroundHighlightBrush);
        }
        
        private void HandleDisabledState()
        {    
            var theme   = ThemeSystem.Instance.Theme;

            _backgroundBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.SlateGrayDisabled]);
            _borderBrush     ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.SlateGrayDisabled]);
            _foregroundBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundInactive]);

            _bd.Background  = _backgroundBrush;
            _bd.BorderBrush = _borderBrush;
            SetForeground(_foregroundBrush);
        }

        protected override void GetTemplateChildOverride(ITemplatePartFinder finder)
        {
            finder.Find<Border>(PART_BdName, x => _bd                     = x)
                  .Find<ContentPresenter>(PART_ContentName, x => _content = x)
                  .Find<Path>(PART_IconName, x => _icon                   = x);
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
    }
}
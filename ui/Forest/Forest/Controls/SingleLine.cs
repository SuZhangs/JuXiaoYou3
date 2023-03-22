using System.Windows.Documents;
using System.Windows.Media.Animation;

namespace Acorisoft.FutureGL.Forest.Controls
{
    public class SingleLine : ForestTextBoxBase
    {
        public static readonly DependencyProperty HeaderProperty;
        public static readonly DependencyProperty HeaderTemplateProperty;
        public static readonly DependencyProperty HeaderTemplateSelectorProperty;
        public static readonly DependencyProperty HeaderStringFormatProperty;
        private const          string             PART_WatermarkName = "PART_Watermark";
        
        static SingleLine()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SingleLine), new FrameworkPropertyMetadata(typeof(SingleLine)));
            
            HeaderProperty = DependencyProperty.Register(
                nameof(Header),
                typeof(object),
                typeof(SingleLine),
                new PropertyMetadata(default(object)));
            
            HeaderTemplateProperty = DependencyProperty.Register(
                nameof(HeaderTemplate),
                typeof(DataTemplate),
                typeof(SingleLine),
                new PropertyMetadata(default(DataTemplate)));

            HeaderStringFormatProperty = DependencyProperty.Register(
                nameof(HeaderStringFormat),
                typeof(string),
                typeof(SingleLine),
                new PropertyMetadata(default(string)));
            
            HeaderTemplateSelectorProperty = DependencyProperty.Register(
                nameof(HeaderTemplateSelector),
                typeof(DataTemplateSelector),
                typeof(SingleLine),
                new PropertyMetadata(default(DataTemplateSelector)));
        }

        private TextBlock       _watermark;
        private Storyboard      _storyboard;
        private SolidColorBrush _borderBrush;
        private SolidColorBrush _background;
        private SolidColorBrush _foreground;
        private SolidColorBrush _backgroundHighlight1;
        private SolidColorBrush _backgroundHighlight2;
        private SolidColorBrush _foregroundHighlight;
        private SolidColorBrush _backgroundDisabled;
        private SolidColorBrush _highlight;
        private SolidColorBrush _foregroundDisabled;

        protected override void GetTemplateChildOverride(ITemplatePartFinder finder)
        {
            finder.Find<Border>(PART_BdName, x => PART_Bd                 = x)
                  .Find<TextBlock>(PART_WatermarkName, x => _watermark    = x)
                  .Find<ScrollViewer>(PART_ContentName, x => PART_Content = x);
        }

        protected override void StopAnimation()
        {
            _storyboard?.Stop(PART_Bd);
        }

        protected override void SetForeground(Brush brush)
        {
            PART_Bd.SetValue(TextElement.ForegroundProperty, brush);
        }

        protected override void OnInvalidateState()
        {
            _borderBrush          = null;
            _background           = null;
            _foreground           = null;
            _backgroundHighlight1 = null;
            _foregroundHighlight  = null;
            _backgroundHighlight2 = null;
            _backgroundDisabled   = null;
            _foregroundDisabled   = null;
            _highlight            = null;
            InvalidateVisual();
        }

        protected override void GoToNormalState(ForestThemeSystem theme)
        {
            _background  ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel3]);
            _borderBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BorderBrush]);
            _foreground  ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundLevel3]);
            
            PART_Bd.Background      = _background;
            PART_Bd.BorderBrush     = _borderBrush;
            PART_Bd.BorderThickness = BorderThickness;
            _watermark.Foreground   = _foreground;
            SetForeground(_foreground);
        }

        protected override void GoToHighlight1State(Duration duration, ForestThemeSystem theme)
        {
            _backgroundHighlight1 ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel2]);
            _foregroundHighlight  ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundLevel1]);
            _highlight            ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA3]);
            
            // 白底变特殊色
            // 高亮色变白色
            // var backgroundAnimation = new ColorAnimation
            // {
            //     Duration = duration,
            //     From     = _background.Color,
            //     To       = _backgroundHighlight1.Color,
            // };
            
            var borderAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _borderBrush.Color,
                To       = _highlight.Color,
            };

            // Storyboard.SetTarget(backgroundAnimation, PART_Bd);
            // Storyboard.SetTargetProperty(backgroundAnimation, new PropertyPath("(Border.Background).(SolidColorBrush.Color)"));
            Storyboard.SetTarget(borderAnimation, PART_Bd);
            Storyboard.SetTargetProperty(borderAnimation, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));

            _storyboard = new Storyboard
            {
                Children = new TimelineCollection { /*backgroundAnimation,*/ borderAnimation }
            };

            //
            // 开始动画
            PART_Bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);

            // 设置文本颜色
            SetForeground(_foregroundHighlight);
        }

        protected override void GoToHighlight2State(Duration duration, ForestThemeSystem theme)
        {
            _backgroundHighlight1 ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel2]);
            _backgroundHighlight2 ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel2]);
            _foregroundHighlight  ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundLevel1]);
            _highlight            ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA3]);
            
            // 白底变特殊色
            // 高亮色变白色
            var backgroundAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _backgroundHighlight1.Color,
                To       = _backgroundHighlight2.Color,
            };
            
            var borderAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _backgroundHighlight1.Color,
                To       = _highlight.Color,
            };

            Storyboard.SetTarget(backgroundAnimation, PART_Bd);
            Storyboard.SetTarget(borderAnimation, PART_Bd);
            Storyboard.SetTargetProperty(borderAnimation, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));
            Storyboard.SetTargetProperty(backgroundAnimation, new PropertyPath("(Border.Background).(SolidColorBrush.Color)"));

            _storyboard = new Storyboard
            {
                Children = new TimelineCollection { backgroundAnimation, borderAnimation }
            };

            //
            // 开始动画
            PART_Bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);

            // 设置文本颜色
            PART_Bd.BorderThickness = BorderThickness;
            _watermark.Foreground   = _highlight;
            CaretBrush              = new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA5]);
            SelectionBrush          = new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA2]);
            SetForeground(_foregroundHighlight);
        }

        protected override void GoToDisableState(ForestThemeSystem theme)
        {
            _backgroundDisabled ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundDisabled]);
            _foregroundDisabled ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundLevel3]);
            
            PART_Bd.Background = _backgroundDisabled;
            SetForeground(_foregroundDisabled);
        }

        /// <summary>
        /// 获取或设置 <see cref="HeaderStringFormat"/> 属性。
        /// </summary>
        public string HeaderStringFormat
        {
            get => (string)GetValue(HeaderStringFormatProperty);
            set => SetValue(HeaderStringFormatProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="HeaderTemplateSelector"/> 属性。
        /// </summary>
        public DataTemplateSelector HeaderTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(HeaderTemplateSelectorProperty);
            set => SetValue(HeaderTemplateSelectorProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="HeaderTemplate"/> 属性。
        /// </summary>
        public DataTemplate HeaderTemplate
        {
            get => (DataTemplate)GetValue(HeaderTemplateProperty);
            set => SetValue(HeaderTemplateProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="Header"/> 属性。
        /// </summary>
        public object Header
        {
            get => (object)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }
    }
}
using System.Windows.Documents;
using System.Windows.Media.Animation;

namespace Acorisoft.FutureGL.Forest.Controls
{
    
    public class Multiline : ForestTextBoxBase
    {
        static Multiline()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Multiline), new FrameworkPropertyMetadata(typeof(Multiline)));
            
            AcceptsTabProperty.AddOwner(typeof(Multiline))
                              .OverrideMetadata(typeof(Multiline), new FrameworkPropertyMetadata(Boxing.True));
            
            AcceptsReturnProperty.AddOwner(typeof(Multiline))
                                 .OverrideMetadata(typeof(Multiline), new FrameworkPropertyMetadata(Boxing.True));
            
            HeaderProperty = DependencyProperty.Register(
                nameof(Header),
                typeof(object),
                typeof(Multiline),
                new PropertyMetadata(default(object)));
            
            HeaderTemplateProperty = DependencyProperty.Register(
                nameof(HeaderTemplate),
                typeof(DataTemplate),
                typeof(Multiline),
                new PropertyMetadata(default(DataTemplate)));

            HeaderStringFormatProperty = DependencyProperty.Register(
                nameof(HeaderStringFormat),
                typeof(string),
                typeof(Multiline),
                new PropertyMetadata(default(string)));
            
            HeaderTemplateSelectorProperty = DependencyProperty.Register(
                nameof(HeaderTemplateSelector),
                typeof(DataTemplateSelector),
                typeof(Multiline),
                new PropertyMetadata(default(DataTemplateSelector)));
        }

        public static readonly DependencyProperty HeaderProperty;
        public static readonly DependencyProperty HeaderTemplateProperty;
        public static readonly DependencyProperty HeaderTemplateSelectorProperty;
        public static readonly DependencyProperty HeaderStringFormatProperty;
        

        private Storyboard      _storyboard;
        private SolidColorBrush _background;
        private SolidColorBrush _foreground;
        private SolidColorBrush _backgroundHighlight1;
        private SolidColorBrush _backgroundHighlight2;
        private SolidColorBrush _foregroundHighlight;
        private SolidColorBrush _backgroundDisabled;
        private SolidColorBrush _foregroundDisabled;

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
            _background           = null;
            _foreground           = null;
            _backgroundHighlight1 = null;
            _foregroundHighlight  = null;
            _backgroundHighlight2 = null;
            _backgroundDisabled   = null;
            _foregroundDisabled   = null;
            InvalidateVisual();
        }

        protected override void GoToNormalState(ForestThemeSystem theme)
        {
            _background        ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel4]);
            _foreground        ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundLevel3]);
            
            PART_Bd.Background =   _background;
            SetForeground(_foreground);
        }

        protected override void GoToHighlight1State(Duration duration, ForestThemeSystem theme)
        {
            _backgroundHighlight1 ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel4]);
            _foregroundHighlight  ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundLevel1]);
            
            // 白底变特殊色
            // 高亮色变白色
            var backgroundAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _background.Color,
                To       = _backgroundHighlight1.Color,
            };

            Storyboard.SetTarget(backgroundAnimation, PART_Bd);
            Storyboard.SetTargetProperty(backgroundAnimation, new PropertyPath("(Border.Background).(SolidColorBrush.Color)"));

            _storyboard = new Storyboard
            {
                Children = new TimelineCollection { backgroundAnimation }
            };

            //
            // 开始动画
            PART_Bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);

            // 设置文本颜色
            SetForeground(_foregroundHighlight);
        }

        protected override void GoToHighlight2State(Duration duration, ForestThemeSystem theme)
        {
            _backgroundHighlight1 ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel4]);
            _backgroundHighlight2 ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel6]);
            _foregroundHighlight  ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundLevel1]);
            
            // 白底变特殊色
            // 高亮色变白色
            var backgroundAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _backgroundHighlight1.Color,
                To       = _backgroundHighlight2.Color,
            };

            Storyboard.SetTarget(backgroundAnimation, PART_Bd);
            Storyboard.SetTargetProperty(backgroundAnimation, new PropertyPath("(Border.Background).(SolidColorBrush.Color)"));

            _storyboard = new Storyboard
            {
                Children = new TimelineCollection { backgroundAnimation }
            };

            //
            // 开始动画
            PART_Bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);

            // 设置文本颜色
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
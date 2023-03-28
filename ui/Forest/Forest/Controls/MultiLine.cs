﻿using System.Windows.Documents;
using System.Windows.Media.Animation;

namespace Acorisoft.FutureGL.Forest.Controls
{
    public class MultiLine : ForestTextBoxBase
    {
        static MultiLine()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiLine), new FrameworkPropertyMetadata(typeof(MultiLine)));

            AcceptsTabProperty.AddOwner(typeof(MultiLine))
                              .OverrideMetadata(typeof(MultiLine), new FrameworkPropertyMetadata(Boxing.True));

            AcceptsReturnProperty.AddOwner(typeof(MultiLine))
                                 .OverrideMetadata(typeof(MultiLine), new FrameworkPropertyMetadata(Boxing.True));
        }

        private const string PART_WatermarkName = "PART_Watermark";


        private TextBlock       _watermark;
        private Storyboard      _storyboard;
        private SolidColorBrush _borderBrush;
        private SolidColorBrush _background;
        private SolidColorBrush _foreground;
        private SolidColorBrush _backgroundHighlight1;
        private SolidColorBrush _backgroundHighlight2;
        private SolidColorBrush _backgroundDisabled;
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
            _watermark.Foreground = brush;
            PART_Bd.SetValue(TextElement.ForegroundProperty, brush);
        }

        protected override void OnInvalidateState()
        {
            _background           = null;
            _foreground           = null;
            _backgroundHighlight1 = null;
            _backgroundHighlight2 = null;
            _backgroundDisabled   = null;
            _foregroundDisabled   = null;
            InvalidateVisual();
        }

        protected override void GoToNormalState(ForestThemeSystem theme)
        {
            _background  ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel1]);
            _borderBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel4]);
            _foreground  ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInAdaptive]);
            
            PART_Bd.Background  = _background;
            PART_Bd.BorderBrush = _borderBrush;
            SetForeground(_foreground);
        }

        protected override void GoToHighlight1State(Duration duration, ForestThemeSystem theme)
        {
            _backgroundHighlight1 ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA3]);
            
            // 白底变特殊色
            // 高亮色变白色
            var borderBrushAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _borderBrush.Color,
                To       = _backgroundHighlight1.Color,
            };
            
            // var borderAnimation = new ColorAnimation
            // {
            //     Duration = duration,
            //     From     = _borderBrush.Color,
            //     To       = _highlight.Color,
            // };

            Storyboard.SetTarget(borderBrushAnimation, PART_Bd);
            Storyboard.SetTargetProperty(borderBrushAnimation, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));
            // Storyboard.SetTarget(borderAnimation, PART_Bd);
            // Storyboard.SetTargetProperty(borderAnimation, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));

            _storyboard = new Storyboard
            {
                Children = new TimelineCollection { borderBrushAnimation,/*borderAnimation*/  }
            };

            //
            // 开始动画
            PART_Bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);
        }

        protected override void GoToHighlight2State(Duration duration, ForestThemeSystem theme)
        {
            _backgroundHighlight1 ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA3]);
            _backgroundHighlight2 ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA4]);
            
            // 白底变特殊色
            // 高亮色变白色
            // 白底变特殊色
            // 高亮色变白色
            var borderBrushAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _borderBrush.Color,
                To       = _backgroundHighlight1.Color,
            };
            
            // var borderAnimation = new ColorAnimation
            // {
            //     Duration = duration,
            //     From     = _borderBrush.Color,
            //     To       = _highlight.Color,
            // };

            Storyboard.SetTarget(borderBrushAnimation, PART_Bd);
            Storyboard.SetTargetProperty(borderBrushAnimation, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));
            // Storyboard.SetTarget(borderAnimation, PART_Bd);
            // Storyboard.SetTargetProperty(borderAnimation, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));

            _storyboard = new Storyboard
            {
                Children = new TimelineCollection { borderBrushAnimation,/*borderAnimation*/  }
            };

            //
            // 开始动画
            PART_Bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);
            SelectionBrush          = new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA2]);
        }

        protected override void GoToDisableState(ForestThemeSystem theme)
        {
            _backgroundDisabled ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel3]);
            _borderBrush        ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BorderBrush]);
            _foregroundDisabled ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundLevel3]);
            
            PART_Bd.Background      = _backgroundDisabled;
            PART_Bd.Background      = _background;
            PART_Bd.BorderBrush     = _borderBrush;
            SetForeground(_foregroundDisabled);
        }
    }


    public class HeaderedMultiLine : ForestTextBoxBase
    {
        public static readonly DependencyProperty HeaderProperty;
        public static readonly DependencyProperty HeaderTemplateProperty;
        public static readonly DependencyProperty HeaderTemplateSelectorProperty;
        public static readonly DependencyProperty HeaderStringFormatProperty;

        static HeaderedMultiLine()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HeaderedMultiLine), new FrameworkPropertyMetadata(typeof(HeaderedMultiLine)));

            AcceptsTabProperty.AddOwner(typeof(HeaderedMultiLine))
                              .OverrideMetadata(typeof(HeaderedMultiLine), new FrameworkPropertyMetadata(Boxing.True));

            AcceptsReturnProperty.AddOwner(typeof(HeaderedMultiLine))
                                 .OverrideMetadata(typeof(HeaderedMultiLine), new FrameworkPropertyMetadata(Boxing.True));
            HeaderStringFormatProperty = DependencyProperty.Register(
                nameof(HeaderStringFormat),
                typeof(string),
                typeof(HeaderedMultiLine),
                new PropertyMetadata(default(string)));

            HeaderProperty = DependencyProperty.Register(
                nameof(Header),
                typeof(object),
                typeof(HeaderedMultiLine),
                new PropertyMetadata(default(object)));
            HeaderTemplateProperty = DependencyProperty.Register(
                nameof(HeaderTemplate),
                typeof(DataTemplate),
                typeof(HeaderedMultiLine),
                new PropertyMetadata(default(DataTemplate)));
            HeaderTemplateSelectorProperty = DependencyProperty.Register(
                nameof(HeaderTemplateSelector),
                typeof(DataTemplateSelector),
                typeof(HeaderedMultiLine),
                new PropertyMetadata(default(DataTemplateSelector)));
        }

        #region Visual

        private Storyboard      _storyboard;
        private SolidColorBrush _borderBrush;
        private SolidColorBrush _background;
        private SolidColorBrush _foreground;
        private SolidColorBrush _backgroundHighlight1;
        private SolidColorBrush _backgroundHighlight2;
        private SolidColorBrush _backgroundDisabled;
        private SolidColorBrush _foregroundDisabled;

        protected override void GetTemplateChildOverride(ITemplatePartFinder finder)
        {
            finder.Find<Border>(PART_BdName, x => PART_Bd                 = x)
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
            _background           = null;
            _foreground           = null;
            _backgroundHighlight1 = null;
            _backgroundHighlight2 = null;
            _backgroundDisabled   = null;
            _foregroundDisabled   = null;
            InvalidateVisual();
        }
        
        protected override void GoToNormalState(ForestThemeSystem theme)
        {
            _background  ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel1]);
            _borderBrush ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel4]);
            _foreground  ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundInAdaptive]);
            
            PART_Bd.Background  = _background;
            PART_Bd.BorderBrush = _borderBrush;
            SetForeground(_foreground);
        }

        protected override void GoToHighlight1State(Duration duration, ForestThemeSystem theme)
        {
            _backgroundHighlight1 ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA3]);
            
            // 白底变特殊色
            // 高亮色变白色
            var borderBrushAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _borderBrush.Color,
                To       = _backgroundHighlight1.Color,
            };
            
            // var borderAnimation = new ColorAnimation
            // {
            //     Duration = duration,
            //     From     = _borderBrush.Color,
            //     To       = _highlight.Color,
            // };

            Storyboard.SetTarget(borderBrushAnimation, PART_Bd);
            Storyboard.SetTargetProperty(borderBrushAnimation, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));
            // Storyboard.SetTarget(borderAnimation, PART_Bd);
            // Storyboard.SetTargetProperty(borderAnimation, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));

            _storyboard = new Storyboard
            {
                Children = new TimelineCollection { borderBrushAnimation,/*borderAnimation*/  }
            };

            //
            // 开始动画
            PART_Bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);
        }

        protected override void GoToHighlight2State(Duration duration, ForestThemeSystem theme)
        {
            _backgroundHighlight1 ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA3]);
            _backgroundHighlight2 ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA4]);
            
            // 白底变特殊色
            // 高亮色变白色
            // 白底变特殊色
            // 高亮色变白色
            var borderBrushAnimation = new ColorAnimation
            {
                Duration = duration,
                From     = _borderBrush.Color,
                To       = _backgroundHighlight1.Color,
            };
            
            // var borderAnimation = new ColorAnimation
            // {
            //     Duration = duration,
            //     From     = _borderBrush.Color,
            //     To       = _highlight.Color,
            // };

            Storyboard.SetTarget(borderBrushAnimation, PART_Bd);
            Storyboard.SetTargetProperty(borderBrushAnimation, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));
            // Storyboard.SetTarget(borderAnimation, PART_Bd);
            // Storyboard.SetTargetProperty(borderAnimation, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));

            _storyboard = new Storyboard
            {
                Children = new TimelineCollection { borderBrushAnimation,/*borderAnimation*/  }
            };

            //
            // 开始动画
            PART_Bd.BeginStoryboard(_storyboard, HandoffBehavior.SnapshotAndReplace, true);
            SelectionBrush          = new SolidColorBrush(theme.Colors[(int)ForestTheme.HighlightA2]);
        }

        protected override void GoToDisableState(ForestThemeSystem theme)
        {
            _backgroundDisabled ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BackgroundLevel3]);
            _borderBrush        ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.BorderBrush]);
            _foregroundDisabled ??= new SolidColorBrush(theme.Colors[(int)ForestTheme.ForegroundLevel3]);
            
            PART_Bd.Background      = _backgroundDisabled;
            PART_Bd.Background      = _background;
            PART_Bd.BorderBrush     = _borderBrush;
            SetForeground(_foregroundDisabled);
        }

        #endregion


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
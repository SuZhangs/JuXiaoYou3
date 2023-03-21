﻿using System.Windows.Media.Animation;

namespace Acorisoft.FutureGL.Forest.Controls
{
    public class SingleLine : ForestTextBoxBase
    {
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
        
        private Storyboard      _storyboard;
        private SolidColorBrush _backgroundBrush;
        private SolidColorBrush _foregroundBrush;
        private SolidColorBrush _backgroundHighlight1Brush;
        private SolidColorBrush _backgroundHighlight2Brush;
        private SolidColorBrush _foregroundHighlightBrush;
        private SolidColorBrush _backgroundDisabledBrush;
        private SolidColorBrush _foregroundDisabledBrush;

        protected override void StopAnimation()
        {
            _storyboard?.Stop(PART_Bd);
        }

        protected override void SetForeground(Brush brush)
        {
            throw new NotImplementedException();
        }

        protected override void OnInvalidateState()
        {
            throw new NotImplementedException();
        }

        protected override void GoToNormalState(ForestThemeSystem theme)
        {
            throw new NotImplementedException();
        }

        protected override void GoToHighlight1State(Duration duration, ForestThemeSystem theme)
        {
            throw new NotImplementedException();
        }

        protected override void GoToHighlight2State(Duration duration, ForestThemeSystem theme)
        {
            throw new NotImplementedException();
        }

        protected override void GoToDisableState(ForestThemeSystem theme)
        {
            throw new NotImplementedException();
        }
        
        public static readonly DependencyProperty HeaderProperty;
        public static readonly DependencyProperty HeaderTemplateProperty;
        public static readonly DependencyProperty HeaderTemplateSelectorProperty;
        public static readonly DependencyProperty HeaderStringFormatProperty;

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
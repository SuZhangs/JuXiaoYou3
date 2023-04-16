using System.ComponentModel;
using System.Windows.Markup;

namespace Acorisoft.FutureGL.Forest.Controls
{
    [DefaultProperty("Content")]
    [ContentProperty("Content")]
    public class EmptyContentControl : ContentControl
    {
        static EmptyContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EmptyContentControl), new FrameworkPropertyMetadata(typeof(EmptyContentControl)));
        }

        /// <summary>
        /// 实现 <see cref="EmptyState"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty EmptyStateProperty = DependencyProperty.Register(
            nameof(EmptyState),
            typeof(object),
            typeof(EmptyContentControl),
            new PropertyMetadata(default(object)));

        /// <summary>
        /// 实现 <see cref="EmptyStateTemplate"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty EmptyStateTemplateProperty = DependencyProperty.Register(
            nameof(EmptyStateTemplate),
            typeof(DataTemplate),
            typeof(EmptyContentControl),
            new PropertyMetadata(default(DataTemplate)));

        /// <summary>
        /// 实现 <see cref="EmptyStateTemplateSelector"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty EmptyStateTemplateSelectorProperty = DependencyProperty.Register(
            nameof(EmptyStateTemplateSelector),
            typeof(DataTemplateSelector),
            typeof(EmptyContentControl),
            new PropertyMetadata(default(DataTemplateSelector)));

        /// <summary>
        /// 实现 <see cref="EmptyStateStringFormat"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty EmptyStateStringFormatProperty = DependencyProperty.Register(
            nameof(EmptyStateStringFormat),
            typeof(string),
            typeof(EmptyContentControl),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty EmptyStateBrushProperty = DependencyProperty.Register(
            nameof(EmptyStateBrush),
            typeof(Brush),
            typeof(EmptyContentControl),
            new PropertyMetadata(default(Brush)));

        public Brush EmptyStateBrush
        {
            get => (Brush)GetValue(EmptyStateBrushProperty);
            set => SetValue(EmptyStateBrushProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="EmptyStateStringFormat"/> 属性。
        /// </summary>
        public string EmptyStateStringFormat
        {
            get => (string)GetValue(EmptyStateStringFormatProperty);
            set => SetValue(EmptyStateStringFormatProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="EmptyStateTemplateSelector"/> 属性。
        /// </summary>
        public DataTemplateSelector EmptyStateTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(EmptyStateTemplateSelectorProperty);
            set => SetValue(EmptyStateTemplateSelectorProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="EmptyStateTemplate"/> 属性。
        /// </summary>
        public DataTemplate EmptyStateTemplate
        {
            get => (DataTemplate)GetValue(EmptyStateTemplateProperty);
            set => SetValue(EmptyStateTemplateProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="EmptyState"/> 属性。
        /// </summary>
        public object EmptyState
        {
            get => (object)GetValue(EmptyStateProperty);
            set => SetValue(EmptyStateProperty, value);
        }   
    }
}
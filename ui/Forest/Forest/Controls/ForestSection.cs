namespace Acorisoft.FutureGL.Forest.Controls
{
    public class ForestSection : ContentControl
    {
        public static readonly DependencyProperty HeaderThicknessProperty = DependencyProperty.Register(
            nameof(HeaderThickness),
            typeof(Thickness),
            typeof(ForestSection),
            new PropertyMetadata(default(Thickness)));

        /// <summary>
        /// 实现 <see cref="Header"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            nameof(Header),
            typeof(object),
            typeof(ForestSection),
            new PropertyMetadata(default(object)));

        /// <summary>
        /// 实现 <see cref="HeaderTemplate"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
            nameof(HeaderTemplate),
            typeof(DataTemplate),
            typeof(ForestSection),
            new PropertyMetadata(default(DataTemplate)));

        /// <summary>
        /// 实现 <see cref="HeaderTemplateSelector"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateSelectorProperty = DependencyProperty.Register(
            nameof(HeaderTemplateSelector),
            typeof(DataTemplateSelector),
            typeof(ForestSection),
            new PropertyMetadata(default(DataTemplateSelector)));

        /// <summary>
        /// 实现 <see cref="HeaderStringFormat"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty HeaderStringFormatProperty = DependencyProperty.Register(
            nameof(HeaderStringFormat),
            typeof(string),
            typeof(ForestSection),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty HeaderPaddingProperty = DependencyProperty.Register(
            nameof(HeaderPadding),
            typeof(Thickness),
            typeof(ForestSection),
            new PropertyMetadata(default(Thickness)));


        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(ForestSection),
            new PropertyMetadata(default(CornerRadius)));


        /// <summary>
        /// 实现 <see cref="ToolBar"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty ToolBarProperty = DependencyProperty.Register(
            nameof(ToolBar),
            typeof(object),
            typeof(ForestSection),
            new PropertyMetadata(default(object)));

        /// <summary>
        /// 实现 <see cref="ToolBarTemplate"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty ToolBarTemplateProperty = DependencyProperty.Register(
            nameof(ToolBarTemplate),
            typeof(DataTemplate),
            typeof(ForestSection),
            new PropertyMetadata(default(DataTemplate)));

        /// <summary>
        /// 实现 <see cref="ToolBarTemplateSelector"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty ToolBarTemplateSelectorProperty = DependencyProperty.Register(
            nameof(ToolBarTemplateSelector),
            typeof(DataTemplateSelector),
            typeof(ForestSection),
            new PropertyMetadata(default(DataTemplateSelector)));

        /// <summary>
        /// 实现 <see cref="ToolBarStringFormat"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty ToolBarStringFormatProperty = DependencyProperty.Register(
            nameof(ToolBarStringFormat),
            typeof(string),
            typeof(ForestSection),
            new PropertyMetadata(default(string)));


        static ForestSection()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ForestSection), new FrameworkPropertyMetadata(typeof(ForestSection)));
        }

        /// <summary>
        /// 获取或设置 <see cref="ToolBarStringFormat"/> 属性。
        /// </summary>
        public string ToolBarStringFormat
        {
            get => (string)GetValue(ToolBarStringFormatProperty);
            set => SetValue(ToolBarStringFormatProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="ToolBarTemplateSelector"/> 属性。
        /// </summary>
        public DataTemplateSelector ToolBarTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(ToolBarTemplateSelectorProperty);
            set => SetValue(ToolBarTemplateSelectorProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="ToolBarTemplate"/> 属性。
        /// </summary>
        public DataTemplate ToolBarTemplate
        {
            get => (DataTemplate)GetValue(ToolBarTemplateProperty);
            set => SetValue(ToolBarTemplateProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="ToolBar"/> 属性。
        /// </summary>
        public object ToolBar
        {
            get => (object)GetValue(ToolBarProperty);
            set => SetValue(ToolBarProperty, value);
        }   
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        
        public Thickness HeaderPadding
        {
            get => (Thickness)GetValue(HeaderPaddingProperty);
            set => SetValue(HeaderPaddingProperty, value);
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
        public Thickness HeaderThickness
        {
            get => (Thickness)GetValue(HeaderThicknessProperty);
            set => SetValue(HeaderThicknessProperty, value);
        }
    }
}
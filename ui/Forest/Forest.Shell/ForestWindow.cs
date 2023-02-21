using System.Windows.Controls;
using System.Windows.Input;

namespace Acorisoft.FutureGL.Forest
{
    public abstract class ForestWindow : Window
    {
        protected ForestWindow()
        {
            Loaded   += OnLoadedIntern;
            Unloaded += OnUnloadedIntern;
            
            //
            //
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, OnWindowClose));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, OnWindowMinimum));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, OnWindowRestore));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, OnWindowRestore));
            
            Style ??= Application.Current.Resources[nameof(ForestWindow)] as Style;
        }
        
        
        #region SystemCommands

        private void OnWindowClose(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void OnWindowMinimum(object sender, ExecutedRoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void OnWindowRestore(object sender, ExecutedRoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        #endregion SystemCommands

        private void OnLoadedIntern(object sender, RoutedEventArgs e)
        {
            //
            // 注册上下文依赖的服务
            (Application.Current as ForestApp)?.RegisterContextServices(Xaml.Container);
            OnLoaded(sender, e);
        }
        
        private void OnUnloadedIntern(object sender, RoutedEventArgs e)
        {
            OnUnloaded(sender, e);
        }
        
        
        protected virtual void OnUnloaded(object sender, RoutedEventArgs e)
        {
        }

        protected virtual void OnLoaded(object sender, RoutedEventArgs e)
        {
        }
        
        public static readonly DependencyProperty ShellContentProperty = DependencyProperty.Register(
            nameof(ShellContent),
            typeof(object),
            typeof(ForestWindow),
            new PropertyMetadata(default(object)));

        public static readonly DependencyProperty ExtendToTitleBarProperty = DependencyProperty.Register(
            nameof(ExtendToTitleBar),
            typeof(bool),
            typeof(ForestWindow),
            new PropertyMetadata(Boxing.True));
        /// <summary>
        /// 实现 <see cref="TitleBar"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty TitleBarProperty = DependencyProperty.Register(
            nameof(TitleBar),
            typeof(object),
            typeof(ForestWindow),
            new PropertyMetadata(default(object)));

        /// <summary>
        /// 实现 <see cref="TitleBarTemplate"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty TitleBarTemplateProperty = DependencyProperty.Register(
            nameof(TitleBarTemplate),
            typeof(DataTemplate),
            typeof(ForestWindow),
            new PropertyMetadata(default(DataTemplate)));

        /// <summary>
        /// 实现 <see cref="TitleBarTemplateSelector"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty TitleBarTemplateSelectorProperty = DependencyProperty.Register(
            nameof(TitleBarTemplateSelector),
            typeof(DataTemplateSelector),
            typeof(ForestWindow),
            new PropertyMetadata(default(DataTemplateSelector)));

        /// <summary>
        /// 实现 <see cref="TitleBarStringFormat"/> 属性的依赖属性。
        /// </summary>
        public static readonly DependencyProperty TitleBarStringFormatProperty = DependencyProperty.Register(
            nameof(TitleBarStringFormat),
            typeof(string),
            typeof(ForestWindow),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            nameof(Color),
            typeof(Brush),
            typeof(ForestWindow),
            new PropertyMetadata(default(Brush)));

        /// <summary>
        /// 获取或设置 <see cref="TitleBarStringFormat"/> 属性。
        /// </summary>
        public Brush Color
        {
            get => (Brush)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="TitleBarStringFormat"/> 属性。
        /// </summary>
        public string TitleBarStringFormat
        {
            get => (string)GetValue(TitleBarStringFormatProperty);
            set => SetValue(TitleBarStringFormatProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="TitleBarTemplateSelector"/> 属性。
        /// </summary>
        public DataTemplateSelector TitleBarTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(TitleBarTemplateSelectorProperty);
            set => SetValue(TitleBarTemplateSelectorProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="TitleBarTemplate"/> 属性。
        /// </summary>
        public DataTemplate TitleBarTemplate
        {
            get => (DataTemplate)GetValue(TitleBarTemplateProperty);
            set => SetValue(TitleBarTemplateProperty, value);
        }

        /// <summary>
        /// 获取或设置 <see cref="TitleBar"/> 属性。
        /// </summary>
        public object TitleBar
        {
            get => GetValue(TitleBarProperty);
            set => SetValue(TitleBarProperty, value);
        }
        
        public bool ExtendToTitleBar
        {
            get => (bool)GetValue(ExtendToTitleBarProperty);
            set => SetValue(ExtendToTitleBarProperty, Boxing.Box(value));
        }
        public object ShellContent
        {
            get => (object)GetValue(ShellContentProperty);
            set => SetValue(ShellContentProperty, value);
        }
    }
}
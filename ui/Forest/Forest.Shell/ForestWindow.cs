namespace Acorisoft.FutureGL.Forest
{
    public abstract class ForestWindow : Window
    {
        protected ForestWindow()
        {
            this.Loaded += OnLoadedIntern;
        }

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
        
    }
}
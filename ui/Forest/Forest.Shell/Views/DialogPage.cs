using Acorisoft.FutureGL.Forest.Contracts;

namespace Acorisoft.FutureGL.Forest.Views
{
    public abstract class DialogPage : UserControl
    {
        protected DialogPage()
        {
            this.Loaded   += OnLoadedIntern;
            this.Unloaded += OnUnloadedIntern;
        }

        private void OnLoadedIntern(object sender, RoutedEventArgs e)
        {
            if (DataContext is IViewModel dc)
            {
                dc.Start();
                
            }
            OnLoaded(sender, e);
        }
        
        private void OnUnloadedIntern(object sender, RoutedEventArgs e)
        {
            if (DataContext is IViewModel dc)
            {
                dc.Stop();
                
            }
            OnUnloaded(sender, e);
        }
        
        protected virtual void OnLoaded(object sender, RoutedEventArgs e){}
        protected virtual void OnUnloaded(object sender, RoutedEventArgs e){}
    }
}
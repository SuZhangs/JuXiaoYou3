using Acorisoft.FutureGL.MigaStudio.Resources.Panels;
using Acorisoft.FutureGL.MigaStudio.ViewModels;
using Microsoft.Xaml.Behaviors;

namespace Acorisoft.FutureGL.MigaStudio.Resources.Behaviors
{
    public class RefreshTabItemSizeBehavior : Behavior<ItemsControl>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            nameof(ViewModel),
            typeof(TabController),
            typeof(RefreshTabItemSizeBehavior),
            new PropertyMetadata(default(TabController)));

        private IDisposable _disposable;

        protected override void OnAttached()
        {
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel is null)
            {
                return;
            }
        }

        private void OnWindowStateChanged(object sender, WindowState e)
        {
        }

        protected override void OnDetaching()
        {
            _disposable?.Dispose();
        }

        public TabController ViewModel
        {
            get => (TabController)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }
    }
}
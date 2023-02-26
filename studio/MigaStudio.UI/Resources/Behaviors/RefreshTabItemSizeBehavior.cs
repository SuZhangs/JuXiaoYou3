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

            var workSpaceSize = SystemParameters.WorkArea.Width;
            
            AssociatedObject.SetValue(TabItemPanel.MaxSizeProperty, (int)(workSpaceSize - 300));
            AssociatedObject.SetValue(TabItemPanel.MinSizeProperty, (int)Math.Clamp(workSpaceSize * 0.8, 1280, 3840) - 300);
            AssociatedObject.Loaded += OnLoaded;
            if (Application.Current.MainWindow is ForestWindow fw)
            {
                fw.WindowStateChanged += OnWindowStateChanged;
            }

        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel is null)
            {
                return;
            }
            
            _disposable = ViewModel.ItemsChanged.Subscribe(_ =>
            {
                AssociatedObject.InvalidateMeasure();
            });
        }

        private void OnWindowStateChanged(object sender, WindowState e)
        {
            AssociatedObject.SetValue(TabItemPanel.WindowStateProperty, e);
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
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
            if (ViewModel is null)
            {
                return;
            }

            _disposable =ViewModel.ItemsChanged.Subscribe(_ =>
            {
                AssociatedObject.InvalidateMeasure();
            });
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
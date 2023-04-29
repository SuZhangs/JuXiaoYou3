using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages.Composes
{
    [Connected(View = typeof(ComposeEditorPage), ViewModel = typeof(ComposeEditorViewModel))]
    public partial class ComposeEditorPage
    {
        private IDisposable _disposable;

        public ComposeEditorPage()
        {
            InitializeComponent();
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            _disposable = Observable.FromEventPattern<EventHandler, EventArgs>(
                                        added => Editor.TextChanged   += added,
                                        removed => Editor.TextChanged -= removed)
                                    .Throttle(TimeSpan.FromMilliseconds(200))
                                    .ObserveOn(Xaml.Get<IScheduler>())
                                    .Subscribe(_ => { ViewModel<ComposeEditorViewModel>().Content = Editor.Text; });
            
            //
            // 初始化
            Editor.Text = ViewModel<ComposeEditorViewModel>().Content;
            base.OnLoaded(sender, e);
        }

        protected override void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _disposable?.Dispose();
            base.OnUnloaded(sender, e);
        }
    }
}
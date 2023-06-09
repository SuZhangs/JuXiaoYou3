using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using Acorisoft.FutureGL.MigaStudio.Controls.Editors;

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
            //
            // 
            ViewModel.Register(Editor)
                     .Register(RtfEditor)
                     .Initialize();
            
            base.OnLoaded(sender, e);
        }

        protected ComposeEditorViewModel ViewModel => ViewModel<ComposeEditorViewModel>();

        protected override void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _disposable?.Dispose();
            base.OnUnloaded(sender, e);
        }
    }
}
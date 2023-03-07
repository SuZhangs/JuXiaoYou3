using System.Windows;
using System.Windows.Controls;
using Acorisoft.FutureGL.Forest;
using Acorisoft.FutureGL.Forest.Interfaces;
using Acorisoft.FutureGL.MigaDB.Documents;
using Acorisoft.FutureGL.MigaStudio.Pages.Gallery;

namespace Acorisoft.FutureGL.Demo.ViewHost.Views
{
    [Connected(View = typeof(ContentHostView), ViewModel = typeof(ContentHostViewModel))]
    public partial class ContentHostView : UserControl
    {
        public ContentHostView()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Xaml.Get<IViewService>().Route(new DocumentGalleryViewModel());
        }
    }
}
using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{

    [Connected(View = typeof(SimpleDocumentGalleryPage), ViewModel = typeof(EasyDocumentGalleryViewModel))]
    public partial class SimpleDocumentGalleryPage
    {
        public SimpleDocumentGalleryPage()
        {
            InitializeComponent();
        }
    }
}
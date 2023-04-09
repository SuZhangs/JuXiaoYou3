using System.Windows.Controls;

namespace Acorisoft.FutureGL.MigaStudio.Pages
{

    [Connected(View = typeof(EasyComposeGalleryPage), ViewModel = typeof(EasyComposeGalleryViewModel))]
    public partial class EasyComposeGalleryPage
    {
        public EasyComposeGalleryPage()
        {
            InitializeComponent();
        }
    }
}